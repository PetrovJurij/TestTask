using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace XMLBookLibrary.Domain
{
    internal class XMLStructureMapper
    {
        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }

        public static XElement CreateXMLStructure(IEnumerable<Book> books)
        {
            var structure = new XElement("booklist");
            foreach (var book in books)
            {
                structure.Add(new XElement(nameof(Book).ToLower(),
                    new XElement(nameof(book.Author).ToLower(), book.Author),
                    new XElement(nameof(book.Title).ToLower(), book.Title),
                    new XElement(nameof(book.PageNumber).ToLower(), book.PageNumber)
                    ));
            }
            return structure;
        }
    }
}
