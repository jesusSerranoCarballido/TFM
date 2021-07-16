
using ClassificationTree;
using ClassificationTree.Infraestructure.Base;
using Synonyms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wrapper.Spacy;

namespace NlpDT{
    public class NLPDT
    {
        private Lang nlp = null;
        public NLPDT() {
            PythonRuntimeUtils.Init("python38.dll", @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python38_64");
            start();
        }
        private void start(string lang = "es_core_news_lg") {
            Spacy spacy = new Spacy();
            nlp = spacy.Load(lang);
        }
        public DataTable Run(DataTable dt, string sentence, IDictionary<string, string> synonyms) {
            var doc = nlp.GetDocument(sentence);
             

            Root main = new Root();
            var syntacticAnalysis = main.Plant(doc.Tokens);
            Synonym synonym = new Synonym();
            CommandSequence.Restrict ps = new CommandSequence.Restrict(synonym.Convert(syntacticAnalysis, dt, synonyms), dt);
            return ps.GetData();
        }

        /// <summary>
        /// Only use for TESTING!
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public IList<(IFunction.function, string)> SentenceTest(string sentence) {
            var doc = nlp.GetDocument(sentence);
            Root main = new Root();
            return main.Plant(doc.Tokens);
        }


        /// <summary>
        /// Generate a web service and create a dependency tree 
        /// </summary>
        /// <param name="sentence"></param>
        public void SyntacticTrees(string sentence) {
            var displacy = new Displacy();
            var doc = nlp.GetDocument(sentence);
            displacy.Serve(doc, "dep");
        }
    }
}
