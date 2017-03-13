using System;

namespace Navigator.Navigation.History
{
    public class HistoryRecord
    {
        public HistoryRecord(Type pageType)
        {
            this.PageName = pageType.FullName;
        }
        public HistoryRecord(string pageName)
        {
            this.PageName = pageName;
        }

        public string PageName { get; private set; }

        public override bool Equals(object obj)
        {
            return this.PageName == (obj as HistoryRecord)?.PageName;
        }

        public override string ToString()
        {
            return this.PageName;
        }
    }
}