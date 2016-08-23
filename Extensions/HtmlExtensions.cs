using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Extensions
{
    public static class HtmlExtensions
    {
        public static List<HtmlElement> GetElementsByClassName(this HtmlElementCollection collection, string className, string tagName = null)
        {
            List<HtmlElement> ret = new List<HtmlElement>();
            foreach (HtmlElement el in collection)
            {
                if (!String.IsNullOrEmpty(el.GetAttribute("className")) && el.GetAttribute("className").ToLower().Trim().Contains(className.ToLower().Trim()))
                {
                    ret.Add(el);
                }
            }
            return ret;
        }

        public static List<HtmlElement> GetElementsByClassName(this HtmlDocument document, string className, string tagName = null)
        {
            if (!String.IsNullOrEmpty(tagName)) return document.GetElementsByTagName(tagName).GetElementsByClassName(className);
            return document.All.GetElementsByClassName(className);
        }

        public static List<HtmlElement> GetElementsByClassName(this HtmlElement element, string className, string tagName = null)
        {   
            List<HtmlElement> ret = new List<HtmlElement>();
            HtmlElementCollection elems;
            if (!String.IsNullOrEmpty(tagName)) elems = element.GetElementsByTagName(tagName);
            else elems = element.All;
            foreach(HtmlElement elem in elems)
            {
                if (!String.IsNullOrEmpty(elem.GetAttribute("className")) && elem.GetAttribute("className").Split(' ').Contains(className.ToLower().Trim()))
                {
                    ret.Add(elem);
                }
            }
            return ret;
        }
    }
}