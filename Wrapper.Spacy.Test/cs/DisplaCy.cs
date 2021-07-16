using System;
using System.Collections.Generic;
using System.Text;
using SpacyDotNet;

namespace Test
{
    static class DisplaCy
    {
        public static void Run()
        {
            var spacy = new Spacy();
            var nlp = spacy.Load("es_core_news_sm");

            var doc = nlp.GetDocument("¿Cuantos años tiene juan?");
            var displacy = new Displacy();
            displacy.Serve(doc, "dep");
        }
    }
}
