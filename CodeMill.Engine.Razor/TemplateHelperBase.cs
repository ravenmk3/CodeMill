using System;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace CodeMill.Engine.Razor
{
    public class TemplateHelperBase
    {
        private TemplateBase _tpl;

        public TemplateHelperBase(TemplateBase template)
        {
            this._tpl = template ?? throw new ArgumentNullException(nameof(template));
        }

        public TemplateBase Template
        {
            get { return this._tpl; }
        }

        protected IEncodedString Raw(string value)
        {
            return this._tpl.Raw(value);
        }

        protected IEncodedString RawFormat(string format, params object[] args)
        {
            return this._tpl.Raw(String.Format(format, args));
        }
    }
}
