﻿using System;
using System.Drawing;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace SS.Ynote.Classic.UI
{
    /// <summary>
    /// Command Input Control
    /// </summary>
    public partial class InputWindow : UserControl
    {
        public delegate void GotInputEventHandler(object sender, InputEventArgs e);
        /// <summary>
        /// Occurs when user has got an input
        /// </summary>
        public event GotInputEventHandler GotInput;
        /// <summary>
        /// Read Only Style
        /// </summary>
        private readonly Style ReadOnlyStyle;
        /// <summary>
        /// Style of the label
        /// </summary>
        private readonly Style LabelStyle;
        /// <summary>
        /// The value of the input
        /// </summary>
        public string InputValue
        {
            get { return tbInput.Text; }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InputWindow()
        {
            InitializeComponent();
            tbInput.KeyDown += tbInput_KeyDown;
            LabelStyle=new TextStyle(Brushes.Blue,null,FontStyle.Regular);
            ReadOnlyStyle = new ReadOnlyStyle();
            tbInput.WideCaret = true;
        }

        void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnInputEntered(new InputEventArgs(InputValue, CaptionText));
                Hide();
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                Hide();
            }
        }

        private string CaptionText;
        /// <summary>
        /// Add a caption
        /// </summary>
        /// <param name="text"></param>
        public void InitInput(string text, GotInputEventHandler handler)
        {
            CaptionText = text;
            var splits = tbInput.Text.Split(':');
            if (splits[0] + ":" == text)
            {
                if (splits[1] == string.Empty) 
                    return;
                tbInput.GoEnd();
                while (tbInput.Selection.CharBeforeStart != ':') 
                    tbInput.Selection.GoLeft(true);
                return;
            }
            tbInput.Text = text;
            tbInput.GoEnd();
            tbInput.Range.ClearStyle();
            tbInput.Range.ClearStyle(ReadOnlyStyle,LabelStyle);
            tbInput.Range.SetStyle(ReadOnlyStyle, @"\b.*:");
            tbInput.Range.SetStyle(LabelStyle, @"\b.*:");
            this.GotInput = null;
            GotInput = handler;
        }
        public virtual void OnInputEntered(InputEventArgs e)
        {
            var handler = GotInput;
            if(handler != null)
                handler(this,e);
        }
        public void Focus()
        {
            tbInput.Focus();
        }
    }
    public class InputEventArgs : EventArgs
    {
        public string Caption;
        /// <summary>
        /// The vale of the Input
        /// </summary>
        public string InputValue;
        /// <summary>
        /// Gets a formatted input seperated by :
        /// </summary>
        /// <returns></returns>
        public string GetFormattedInput()
        {
            if (string.IsNullOrEmpty(Caption))
                return InputValue.Split(':')[1];
            return InputValue.Substring(Caption.Length, InputValue.Length - Caption.Length);
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InputEventArgs(string val, string caption=null)
        {
            this.InputValue = val;
            this.Caption = caption;
        }
    }
}
