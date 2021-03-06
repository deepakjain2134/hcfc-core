using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using HCF.BDO;
using Version = Lucene.Net.Util.Version;
using System.Data;

namespace HCF.BAL
{
    public static class GoLucene    {

        private static FSDirectory _directoryTemp;

        private static FSDirectory _masterdirectoryTemp;


        public static FSDirectory _directory()
        {
            return null;

            //_directoryTemp = FSDirectory.Open(new DirectoryInfo(Utility.Common.GetlucenePath()));
            //if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
            //var lockFilePath = Path.Combine(Utility.Common.GetlucenePath(), "write.lock");
            //if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
            //return _directoryTemp;
        }

        public static FSDirectory _masterdirectory()
        {
            return null;

            //_masterdirectoryTemp = FSDirectory.Open(new DirectoryInfo(Utility.Common.GetMasterLucenePath()));
            //if (IndexWriter.IsLocked(_masterdirectoryTemp)) IndexWriter.Unlock(_masterdirectoryTemp);
            //var lockFilePath = Path.Combine(Utility.Common.GetMasterLucenePath(), "write.lock");
            //if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
            //return _masterdirectoryTemp;
        }



        // search methods
        public static IEnumerable<SampleData> GetAllIndexRecords()
        {
            // validate search index
            //i//f (!System.IO.Directory.EnumerateFiles(Utility.Common.GetlucenePath()).Any()) return new List<SampleData>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory(), false);
            var reader = IndexReader.Open(_directory(), false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            // v 2.9.4: use 'hit.Doc()'
            // v 3.0.3: use 'hit.Doc'
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }
        public static IEnumerable<SampleData> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<SampleData>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return _search(input, fieldName);
        }
        public static IEnumerable<SampleData> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<SampleData>() : _search(input, fieldName);
        }
        // main search method
        private static IEnumerable<SampleData> _search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SampleData>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory(), false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    // searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "Type", "BarCodeNo", "AssetNo", "BinderName", "IncidentNo" }, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    // searcher.Dispose();
                    return results;
                }
            }
        }
        private static Query parseQuery(string searchQuery, QueryParser parser)
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
        private static IEnumerable<SampleData> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<SampleData> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            // v 2.9.4: use 'hit.doc'
            // v 3.0.3: use 'hit.Doc'
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }
        private static SampleData _mapLuceneDocumentToData(Document doc)
        {
            return new SampleData
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                AssetNo = doc.Get("AssetNo"),
                BarCodeNo = doc.Get("BarCodeNo"),
                BinderName = doc.Get("BinderName"),
                Type = Convert.ToInt32(doc.Get("Type")),
                IncidentNo = doc.Get("IncidentNo")
            };
        }
        // add/update/clear search index data 
        public static void AddUpdateLuceneIndex(SampleData sampleData)
        {
            AddUpdateLuceneIndex(new List<SampleData> { sampleData });
        }
        public static void AddUpdateLuceneIndex(IEnumerable<SampleData> sampleDatas)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory(), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (var sampleData in sampleDatas)
                    _addToLuceneIndex(sampleData, writer);

                // close handles
                analyzer.Close();
                //writer.Dispose();
            }
        }
        public static void ClearLuceneIndexRecord(int record_id)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory(), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                //writer.Dispose();
            }
        }
        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory(), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    //writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory(), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                //writer.Dispose();
            }
        }
        private static void _addToLuceneIndex(SampleData sampleData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", sampleData.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("AssetNo", sampleData.AssetNo, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("BarCodeNo", sampleData.BarCodeNo, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("BinderName", sampleData.BinderName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Type", sampleData.Type.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("IncidentNo", sampleData.IncidentNo, Field.Store.YES, Field.Index.ANALYZED));
            // add entry to index
            writer.AddDocument(doc);
        }
       
        public static void AdEPsLuceneIndex(DataTable sampleDatas)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_masterdirectory(), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (DataRow sampleData in sampleDatas.Rows)
                    _addToEPsLuceneIndex(sampleData, writer);

                // close handles
                analyzer.Close();
                //writer.Dispose();
            }
        }
        private static void _addToEPsLuceneIndex(DataRow sampleData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("EPDetailId", sampleData["EPDetailId"].ToString()));
            writer.DeleteDocuments(searchQuery);
           
            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("StDescID", sampleData["StDescID"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("EPDetailId", sampleData["EPDetailId"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Description", sampleData["Description"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("TJCDescription", sampleData["TJCDescription"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("CategoryId", sampleData["CategoryId"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("AssetName", sampleData["AssetName"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("AssetId", sampleData["AssetId"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("DocTypeId", sampleData["DocTypeId"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("DocTypeName", sampleData["DocTypeName"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("BinderName", sampleData["BinderName"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("BinderId", sampleData["BinderId"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("FrequencyId", sampleData["FrequencyId"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("FrequencyName", sampleData["DisplayName"].ToString(), Field.Store.YES, Field.Index.ANALYZED));           
            // add entry to index
            writer.AddDocument(doc);
        }
    }
}