using System;

using TaskDialogFlags = KPreisser.UI.TaskDialogNativeMethods.TASKDIALOG_FLAGS;

namespace KPreisser.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TaskDialogControl
    {
        // Disallow inheritance by specifying a private protected constructor.
        protected TaskDialogControl()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        internal TaskDialogPage BoundPage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates if the current state of this control
        /// allows it to be created in a task dialog when binding it.
        /// </summary>
        internal virtual bool IsCreatable
        {
            get => true;
        }

        /// <summary>
        /// Gets or sets a value that indicates if this control has been created
        /// in a bound task dialog.
        /// </summary>
        internal bool IsCreated
        {
            get;
            private set;
        }

        internal TaskDialogFlags Bind(TaskDialogPage page)
        {
            BoundPage = page ??
                    throw new ArgumentNullException(nameof(page));

            // Use the current value of IsCreatable to determine if the control is
            // created. This is important because IsCreatable can change while the
            // control is displayed (e.g. if it depends on the Text property).
            IsCreated = IsCreatable;

            return IsCreated ? BindCore() : 0;
        }

        internal void Unbind()
        {
            if (IsCreated)
                UnbindCore();

            IsCreated = false;
            BoundPage = null;
        }

        /// <summary>
        /// Applies initialization after the task dialog is displayed or navigated.
        /// </summary>
        internal void ApplyInitialization()
        {
            // Only apply the initialization if the control is actually created.
            if (IsCreated)
                ApplyInitializationCore();
        }

        /// <summary>
        /// When overridden in a subclass, runs additional binding logic and returns
        /// flags to be specified before the task dialog is displayed or navigated.
        /// </summary>
        /// <remarks>
        /// This method will only be called if <see cref="IsCreatable"/> returns <c>true</c>.
        /// </remarks>
        /// <returns></returns>
        protected virtual TaskDialogFlags BindCore()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This method will only be called if <see cref="BindCore"/> was called.
        /// </remarks>
        protected virtual void UnbindCore()
        {
        }

        /// <summary>
        /// When overridden in a subclass, applies initialization after the task dialog
        /// is displayed or navigated.
        /// </summary>
        /// <remarks>
        /// This method will only be called if <see cref="IsCreatable"/> returns <c>true</c>.
        /// </remarks>
        protected virtual void ApplyInitializationCore()
        {
        }

        protected void DenyIfBound()
        {
            BoundPage?.DenyIfBound();
        }

        protected void DenyIfWaitingForInitialization()
        {
            BoundPage?.DenyIfWaitingForInitialization();
        }

        protected void DenyIfNotBoundOrWaitingForInitialization()
        {
            DenyIfWaitingForInitialization();

            if (BoundPage == null)
                throw new InvalidOperationException(
                        "This control is not currently bound to a task dialog.");
        }

        protected void DenyIfBoundAndNotCreated()
        {
            if (BoundPage != null && !IsCreated)
                throw new InvalidOperationException("The control has not been created.");
        }
    }
}
