using System;
using System.Linq;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace CodeMill.Engine.Razor
{
    public class CodeTemplate<T> : TemplateBase<T>, ITemplate<T>, ITemplate
    {
        #region Indent

        private string _indentString = "    ";

        public void SetIndentString(string s)
        {
            this._indentString = s;
        }

        private string GetIndentString(int count)
        {
            return String.Join("", Enumerable.Repeat(this._indentString, count));
        }

        public IEncodedString Indent()
        {
            return this.Raw(this._indentString);
        }

        public IEncodedString Indent(int count)
        {
            return this.Raw(this.GetIndentString(count));
        }

        #endregion Indent

        #region Line

        public string NewLine()
        {
            return "\r\n";
        }

        public string Line(string value)
        {
            return String.Concat(value, "\r\n");
        }

        public string Line(string format, params object[] args)
        {
            return String.Concat(String.Format(format, args), "\r\n");
        }

        public string Line(int indent, string value)
        {
            return String.Concat(this.GetIndentString(indent), value, "\r\n");
        }

        public string Line(int indent, string format, params object[] args)
        {
            return String.Concat(this.GetIndentString(indent), String.Format(format, args), "\r\n");
        }

        #endregion Line

        #region RawLine

        public IEncodedString RawNewLine()
        {
            return this.Raw("\r\n");
        }

        public IEncodedString RawLine(string value)
        {
            return this.Raw(String.Concat(value, "\r\n"));
        }

        public IEncodedString RawLine(string format, params object[] args)
        {
            var value = String.Concat(String.Format(format, args), "\r\n");
            return this.Raw(value);
        }

        public IEncodedString RawLine(int indent, string value)
        {
            return this.Raw(String.Concat(this.GetIndentString(indent), value, "\r\n"));
        }

        public IEncodedString RawLine(int indent, string format, params object[] args)
        {
            var value = String.Concat(this.GetIndentString(indent), String.Format(format, args), "\r\n");
            return this.Raw(value);
        }

        #endregion RawLine

        #region RawFormat

        public IEncodedString RawFormat(string format, params object[] args)
        {
            var value = String.Format(format, args);
            return this.Raw(value);
        }

        public IEncodedString RawFormat(int indent, string format, params object[] args)
        {
            var value = String.Concat(this.GetIndentString(indent), String.Format(format, args));
            return this.Raw(value);
        }

        #endregion RawFormat
    }
}
