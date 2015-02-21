using System;
using Newtonsoft.Json.Linq;

namespace MaasOne.Net
{
    public abstract class YqlQuery<T> : Query<T> where T : ResultBase
    {
        internal YqlQuery() { }


        protected override T ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            string htmlText = MyHelper.StreamToString(stream);
            JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
            JToken yqlToken = this.YqlTokenFromDirectSource(htmlDoc);
            return this.YqlConvertToken(yqlToken, ci);
        }


        internal abstract T YqlConvertToken(JToken yqlToken, ConvertInfo ci);

        internal abstract JToken YqlTokenFromDirectSource(JObject htmlDoc);

        internal abstract string YqlXPath();
    }
}
