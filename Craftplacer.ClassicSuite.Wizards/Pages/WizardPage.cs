﻿using Craftplacer.ClassicSuite.Wizards.Enums;
using Craftplacer.ClassicSuite.Wizards.Exceptions;
using Craftplacer.ClassicSuite.Wizards.Forms;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Craftplacer.ClassicSuite.Wizards.Pages
{
    public class WizardPage : UserControl
    {
        public override DockStyle Dock => DockStyle.Fill;

        /// <summary>
        /// The title that appears in the header.
        /// </summary>
        [Category("Appearance")]
        [Description("The title that appears in the header.")]
        public virtual string Title { get; set; }

        /// <summary>
        /// The subtitle that appears under the title in the header.
        /// </summary>
        [Category("Appearance")]
        [Description("The subtitle that appears under the title in the header.")]
        public virtual string Subtitle { get; set; }

        /// <summary>
        /// The text that appears in the footer beside the buttons.
        /// </summary>
        [Category("Appearance")]
        [Description("The text that appears in the footer beside the buttons.")]
        public virtual string FooterText { get; set; }

        #region Button Texts

        /// <summary>
        /// Text displayed on the back button of the wizard. If empty, it will display the default text.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("Text displayed on the back button of the wizard. If empty, it will display the default text.")]
        public virtual string BackButtonText
        {
            get => backButtonText;
            set
            {
                backButtonText = value;
                ButtonTextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Text displayed on the next button of the wizard. If empty, it will display the default text.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("Text displayed on the next button of the wizard. If empty, it will display the default text.")]
        public virtual string NextButtonText
        {
            get => nextButtonText;
            set
            {
                nextButtonText = value;
                ButtonTextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Text displayed on the cancel button of the wizard. If empty, it will display the default text.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("Text displayed on the cancel button of the wizard. If empty, it will display the default text.")]
        public virtual string CancelButtonText
        {
            get => cancelButtonText;
            set
            {
                cancelButtonText = value;
                ButtonTextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string cancelButtonText;
        private string nextButtonText;
        private string backButtonText;

        public event EventHandler<EventArgs> ButtonTextChanged;

        #endregion Button Texts

        #region Page Parts

        private PageParts pageParts = PageParts.None;

        /// <summary>
        /// What extra parts to show, this can range from sidebar imagery to a small header displaying the title.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(PageParts.None)]
        [Description("What extra parts to show, this can range from sidebar imagery to a small header displaying the title.")]
        public PageParts PageParts
        {
            get => pageParts;
            set
            {
                pageParts = value;
                Size = GetSize();
            }
        }

        /// <summary>
        /// The image that will be displayed in the header.
        /// </summary>
        [Category("Appearance")]
        public Image HeaderImage { get; set; }

        /// <summary>
        /// The image that will be displayed in the sidebar.
        /// </summary>
        [Category("Appearance")]
        public Image SidebarImage { get; set; }

        #endregion Page Parts

        #region Next Page

        /// <summary>
        /// The next page that will be displayed when the user clicks the next button. If set to <see cref="null"/>, the next button will be set to "Finish" and will cause the wizard to close.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WizardPage NextPage { get; set; }

        /// <summary>
        /// Invokes the ParentForm to navigate forwards.
        /// </summary>
        protected void OnNextPageRequested()
        {
            if (ParentForm is WizardForm wizard)
            {
                wizard.NavigateForwards();
            }
            else
            {
                throw new InvalidParentFormException($"The operation is invalid because the {nameof(ParentForm)} is not of type {typeof(WizardForm)}");
            }
        }

        /// <summary>
        /// Invokes the ParentForm to navigate backwards.
        /// </summary>
        protected void OnPreviousPageRequested()
        {
            if (ParentForm is WizardForm wizard)
            {
                wizard.NavigateBackwards();
            }
            else
            {
                throw new InvalidParentFormException($"The operation is invalid because the {nameof(ParentForm)} is not of type {typeof(WizardForm)}");
            }
        }

        #endregion Next Page

        #region Navigation Events

        /// <summary>
        /// Occurs when the parent WizardForm navigates to this page.
        /// </summary>
        public event EventHandler<EventArgs> EnterPage;

        /// <summary>
        /// Occurs when the parent WizardForm navigates away from this page.
        /// </summary>
        public event EventHandler<EventArgs> LeavePage;

        public void OnEnterPage()
        {
            EnterPage?.Invoke(this, EventArgs.Empty);
        }

        public void OnLeavePage()
        {
            LeavePage?.Invoke(this, EventArgs.Empty);
        }

        #endregion Navigation Events

        #region Cancellation

        /// <summary>
        /// Occurs when the parent WizardForm tries to cancel the wizard process.
        /// </summary>
        public event CancelEventHandler CancellationRequested;

        public void OnCancelRequested(CancelEventArgs e)
        {
            CancellationRequested?.Invoke(this, e);
        }

        #endregion

        #region Buttons

        /// <summary>
        /// What buttons to enable, this can be updated at runtime and can be used for asynchronous operations.
        /// </summary>
        private AllowedButton allowedButtons = AllowedButton.All;

        [Category("Behavior")]
        public virtual AllowedButton AllowedButtons
        {
            get => allowedButtons;
            set
            {
                allowedButtons = value;
                AllowedButtonsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> AllowedButtonsChanged;

        #endregion Buttons

        #region Sizing

        private Size GetSize()
        {
            switch (PageParts)
            {
                case PageParts.Header:
                    return new Size(497, 253);

                case PageParts.Sidebar:
                    return new Size(333, 313);

                default:
                    return new Size(497, 313);
            }
        }

        public override bool AutoSize => true;

        public override Size GetPreferredSize(Size proposedSize)
        {
            return GetSize();
        }

        #endregion Sizing
    }
}