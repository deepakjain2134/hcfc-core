using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace HCF.BAL.Services
{
    public class LuceneIndexer
    {
        private Analyzer _analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
        private string _indexPath;
        private Directory _indexDirectory;
        private IndexWriter _indexWriter;


        public LuceneIndexer(string indexPath)
        {
            this._indexPath = indexPath;
            _indexDirectory = new MMapDirectory(new System.IO.DirectoryInfo(_indexPath));
        }

        public void BuildCompleteIndex(IEnumerable<Document> documents)
        {
            _indexWriter = new IndexWriter(_indexDirectory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var doc in documents)
            {
                _indexWriter.AddDocument(doc);
            }

            _indexWriter.Optimize();
            _indexWriter.Flush(true, true, true);
            _indexWriter.Close();
            _indexWriter.Dispose();
        }

        public int UpdateIndex(IEnumerable<Document> documents)
        {
            throw new NotImplementedException();
        }

        public void ClearIndex()
        {
            _indexWriter = new IndexWriter(_indexDirectory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _indexWriter.DeleteAll();
            _indexWriter.Close();
            _indexWriter.Dispose();
        }


        //Single field search
        public IEnumerable<Document> Search(string searchTerm, string searchField, int limit)
        {
            var searcher = new IndexSearcher(_indexDirectory);
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, _analyzer);
            var query = parser.Parse(searchTerm);
            var hits = searcher.Search(query, limit).ScoreDocs;

            var documents = new List<Document>();
            foreach (var hit in hits)
            {
                documents.Add(searcher.Doc(hit.Doc));
            }

            _analyzer.Close();
            searcher.Dispose();
            return documents;
        }

        //Allows multiple field searches
        public IEnumerable<Document> Search(string searchTerm, string[] searchFields, int limit)
        {
            var searcher = new IndexSearcher(_indexDirectory);
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, searchFields, _analyzer);
            var query = parser.Parse(searchTerm);
            var hits = searcher.Search(query, limit).ScoreDocs;

            var documents = new List<Document>();
            foreach (var hit in hits)
            {
                documents.Add(searcher.Doc(hit.Doc));
            }

            _analyzer.Close();
            searcher.Dispose();
            return documents;
        }

    }
}
