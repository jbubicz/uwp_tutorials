using System;
using Windows.UI.Xaml.Controls;

namespace Navigator.PagesManagment
{
    public class PagesManager
    {
        public Page[] Pages { get; private set; }

        public void InitializePages(Page[] pages) => this.Pages = pages;

        public Page GetPageByString(string pageName)
        {
            Page pageResult = null;

            foreach (var page in this.Pages)
            {
                if (page.ToString()==pageName)
                {
                    var a = page.ToString();
                    pageResult = page;
                    break;
                }
            }
            return pageResult;
        }

        public Type GetTypeOfPageByString(string pageName)
        {
            Type type = this.GetPageByString(pageName).GetType();
            return type;
        }
    }
}