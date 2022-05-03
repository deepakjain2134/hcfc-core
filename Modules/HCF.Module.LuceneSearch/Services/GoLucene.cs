using HCF.Module.LuceneSearch.Model;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Version = Lucene.Net.Util.Version;
using HCF.Infrastructure;
using HCF.Module.LuceneSearch.Models;

namespace HCF.Module.LuceneSearch.Services
{
    public class GoLucene : IGoLucene
    {

        private readonly string _luceneDir = Path.Combine(GlobalConfiguration.WebRootPath, "lucene_index");

        public GoLucene()
        {
            //_luceneDir = (string);
        }

        private FSDirectory _directoryTemp;

        private FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        public bool GetDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            return true;
        }

        // search methods
        public IEnumerable<T> GetAllIndexRecords<T>() where T : new()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<T>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            // v 2.9.4: use 'term.Doc()'
            // v 3.0.3: use 'term.Doc'
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList<T>(docs);
        }
        public IEnumerable<T> Search<T>(string input, LuceneFileDirectory fileDirectory, string fieldName = "") where T : new()
        {
            if (string.IsNullOrEmpty(input)) return new List<T>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return _search<T>(input, fieldName);
        }
        public IEnumerable<T> SearchDefault<T>(string input, LuceneFileDirectory fileDirectory, string fieldName = "") where T : new()
        {
            return string.IsNullOrEmpty(input) ? new List<T>() : _search<T>(input, fieldName);
        }

        // main search method
        private IEnumerable<T> _search<T>(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<T>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = _mapLuceneToDataList<T>(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
                    var results = _mapLuceneToDataList<T>(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
        private Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        // map Lucene search index to data
        private IEnumerable<T> _mapLuceneToDataList<T>(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData<T>).ToList();
        }
        private IEnumerable<T> _mapLuceneToDataList<T>(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            // v 2.9.4: use 'hit.doc'
            // v 3.0.3: use 'hit.Doc'
            return hits.Select(hit => _mapLuceneDocumentToData<T>(searcher.Doc(hit.Doc))).ToList();
        }

        private T _mapLuceneDocumentToData<T>(Document doc)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();
            foreach (var pro in temp.GetProperties())
            {
                pro.SetValue(obj, doc.Get(pro.Name), null);

            }
            return obj;
        }

        // add/update/clear search index data 
        public void AddUpdateLuceneIndex<T>(T sampleData) where T : new()
        {
            AddUpdateLuceneIndex(new List<T> { sampleData });
        }
        public void AddUpdateLuceneIndex<T>(IEnumerable<T> sampleDatas) where T : new()
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (var sampleData in sampleDatas)
                    _addToLuceneIndex(sampleData, writer);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }
        public void ClearLuceneIndexRecord(int record_id)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }
        public bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }
        private void _addToLuceneIndex<T>(T sampleData, IndexWriter writer) where T : new()
        {
            //// remove older index entry
            //var searchQuery = new TermQuery(new Term("Id", sampleData.Id.ToString()));
            //writer.DeleteDocuments(searchQuery);

            //// add new index entry
            //var doc = new Document();

            //// add lucene fields mapped to db fields
            //doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //doc.Add(new Field("Name", sampleData.Name, Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("Description", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));

            //// add entry to index
            //writer.AddDocument(doc);
        }

    }
}
