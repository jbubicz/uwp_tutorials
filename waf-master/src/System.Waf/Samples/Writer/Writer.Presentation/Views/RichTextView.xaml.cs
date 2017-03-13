﻿using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using System.ComponentModel;
using System.Collections.Generic;

namespace Waf.Writer.Presentation.Views
{
    [Export(typeof(IRichTextView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RichTextView : UserControl, IRichTextView
    {
        private readonly Lazy<RichTextViewModel> viewModel;
        private bool suppressTextChanged;
        private IReadOnlyList<Control> dynamicContextMenuItems;
        

        public RichTextView()
        {
            InitializeComponent();

            viewModel = new Lazy<RichTextViewModel>(() => ViewHelper.GetViewModel<RichTextViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
            IsVisibleChanged += IsVisibleChangedHandler;
        }


        private RichTextViewModel ViewModel => viewModel.Value;


        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // Ensure that this handler is called only once.
            Loaded -= FirstTimeLoadedHandler;
            
            suppressTextChanged = true;
            richTextBox.Document = ViewModel.Document.Content;
            suppressTextChanged = false;
        }

        private void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.IsVisible = IsVisible;
        }

        private void RichTextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (richTextBox.IsVisible)
            {
                richTextBox.Focus();
            }
        }

        private void RichTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!suppressTextChanged) { ViewModel.Document.Modified = true; }

            UpdateFormattingProperties();
        }

        private void RichTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateFormattingProperties();
        }

        private void UpdateFormattingProperties()
        {
            TextRange selection = richTextBox.Selection;

            object fontWeight = selection.GetPropertyValue(TextElement.FontWeightProperty);
            object fontStyle = selection.GetPropertyValue(TextElement.FontStyleProperty);
            object textDecotations = selection.GetPropertyValue(Inline.TextDecorationsProperty);

            ViewModel.IsBold = fontWeight != DependencyProperty.UnsetValue && (FontWeight)fontWeight != FontWeights.Normal;
            ViewModel.IsItalic = fontStyle != DependencyProperty.UnsetValue && (FontStyle)fontStyle == FontStyles.Italic;
            ViewModel.IsUnderline = textDecotations != DependencyProperty.UnsetValue && textDecotations == TextDecorations.Underline;

            bool isNumberedList = false;
            bool isBulletList = false;
            ListItem listItem = null;
            if (selection.Start.Paragraph != null)
            {
                listItem = selection.Start.Paragraph.Parent as ListItem;
            }
            if (listItem != null)
            {
                List list = (List)listItem.Parent;
                isNumberedList = list.MarkerStyle == TextMarkerStyle.Decimal;
                isBulletList = list.MarkerStyle == TextMarkerStyle.Disc;
            }
            ViewModel.IsNumberedList = isNumberedList;
            ViewModel.IsBulletList = isBulletList;
        }

        private void RichTextBoxContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            List<Control> menuItems = new List<Control>();

            SpellingError spellingError = richTextBox.GetSpellingError(richTextBox.CaretPosition);
            if (spellingError != null)
            {
                foreach (string suggestion in spellingError.Suggestions.Take(5))
                {
                    MenuItem menuItem = new MenuItem()
                    {
                        Header = suggestion,
                        FontWeight = FontWeights.Bold,
                        Command = EditingCommands.CorrectSpellingError,
                        CommandParameter = suggestion
                    };
                    menuItems.Add(menuItem);
                }

                if (!menuItems.Any())
                {
                    MenuItem noSpellingSuggestions = new MenuItem()
                    {
                        Header = Properties.Resources.NoSpellingSuggestions,
                        FontWeight = FontWeights.Bold,
                        IsEnabled = false,
                    };
                    menuItems.Add(noSpellingSuggestions);
                }

                menuItems.Add(new Separator());

                MenuItem ignoreAllMenuItem = new MenuItem()
                {
                    Header = Properties.Resources.IgnoreAllMenu,
                    Command = EditingCommands.IgnoreSpellingError
                };
                menuItems.Add(ignoreAllMenuItem);

                menuItems.Add(new Separator());
            }

            foreach (Control item in menuItems.Reverse<Control>())
            {
                contextMenu.Items.Insert(0, item);
            }

            dynamicContextMenuItems = menuItems;
        }

        private void RichTextBoxContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            foreach (Control menuItem in dynamicContextMenuItems)
            {
                contextMenu.Items.Remove(menuItem);
            }
        }
    }
}
