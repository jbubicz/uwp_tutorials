using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMethodReturnValue.Global

namespace Navigator.Navigation.History
{
    /// <summary>
    /// The class which describes History Object for navigation
    /// </summary>
    public class NavigationHistory
    {

        #region Properties
        /// <summary>
        /// the current index of navigation
        /// </summary>
        private HistoryRecord CurrentType => this.CurrentNode?.Value;
        internal LinkedListNode<HistoryRecord> CurrentNode { get; set; }
        internal LinkedList<HistoryRecord> FrameNavigationHistory { get; set; }
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor without initializing default page
        /// </summary>
        public NavigationHistory()
        {
            this.FrameNavigationHistory = new LinkedList<HistoryRecord>();
        }

        #endregion

        #region Operations on history

        #region Operations TYPE
        /// <summary>
        /// Adds page type to the history
        /// </summary>
        /// <param name="frameType">Adding frame type</param>
        /// <returns>success of the operation</returns>
        internal bool Add(Type frameType)
        {
            bool result = false;
            if (frameType != null)
            {
                this.FrameNavigationHistory.AddLast(new HistoryRecord(frameType));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes specified page type from the page types list (history)
        /// </summary>
        /// <param name="frameType">Removing type</param>
        /// <returns>success of the operation</returns>
        internal bool Remove(Type frameType)
        {
            bool result = this.FrameNavigationHistory.Remove(
                this.FrameNavigationHistory.FirstOrDefault(t => t.PageName == frameType.FullName));

            return result;
        }

        public void ClearAfter(Type frameType)
        {
            LinkedList<HistoryRecord> resultingHistoryRecords =  new LinkedList<HistoryRecord>();

            foreach (var historyRecord in this.FrameNavigationHistory)
            { 
                resultingHistoryRecords.AddLast(historyRecord);

                if (historyRecord.PageName == frameType.FullName)
                {
                    this.FrameNavigationHistory = resultingHistoryRecords;
                    return;
                }
                   
            }
        }
        #endregion

        #region Operations STRING
        /// <summary>
        /// Adds page type to the history
        /// </summary>
        /// <param name="typeName">Adding frame type name</param>
        /// <returns>success of the operation</returns>
        internal bool Add(string typeName)
        {
            bool result = false;
            if (!String.IsNullOrWhiteSpace(typeName))
            {
                this.FrameNavigationHistory.AddLast(new HistoryRecord(typeName));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes specified page type from the page types list (history)
        /// </summary>
        /// <param name="typeName">Removing type name</param>
        /// <returns>success of the operation</returns>
        internal bool Remove(string typeName)
        {
            bool result = this.FrameNavigationHistory.Remove(
                this.FrameNavigationHistory.FirstOrDefault(t => t.PageName == typeName));

            return result;
        }

        public void ClearAfter(string frameName)
        {
            LinkedList<HistoryRecord> resultingHistoryRecords = new LinkedList<HistoryRecord>();

            foreach (var historyRecord in this.FrameNavigationHistory)
            {
                resultingHistoryRecords.AddLast(historyRecord);

                if (historyRecord.PageName == frameName)
                {
                    this.FrameNavigationHistory = resultingHistoryRecords;
                    return;
                }

            }
        }
        #endregion

        /// <summary>
        /// Clears list from values and resets index to -1
        /// </summary>
        public void Clear() => this.FrameNavigationHistory.Clear();

        #endregion

        #region GetInfoAboutHistory

        /// <summary>
        /// Gets the current page type
        /// </summary>
        /// <returns>optained type</returns>
        public HistoryRecord GetCurrentPageType() => this.CurrentType;

        /// <summary>
        /// Counts items in the page types list (history)
        /// </summary>
        /// <returns>ammount of items in the list (history)</returns>
        internal int Count() => this.FrameNavigationHistory.Count;

        /// <summary>
        /// Navigates the next value of the history
        /// </summary>
        /// <returns>the type of page the next item of history</returns>
        internal HistoryRecord GetNext() => this.CurrentNode.Next.Value;

        /// <summary>
        /// Navigates the previous value of the history
        /// </summary>
        /// <returns>the type of page the previous item of history</returns>
        internal HistoryRecord GetPrevious() => this.CurrentNode.Previous.Value;

        #endregion
    }
}