using Xamarin.Forms;

namespace Behaviors.Handlers
{
    [Preserve(AllMembers = true)]
    public sealed class DataHandlerBehavior : BehaviorPropertiesBase
    {
        private VisualElement _associatedView;

        public static readonly BindableProperty IsShownProperty =
            BindableProperty.Create(
                nameof(IsShown),
                typeof(bool),
                typeof(DataHandlerBehavior),
                false,
                propertyChanged: OnDataHandlerBehavior);
        public bool IsShown
        {
            get => (bool)GetValue(IsShownProperty);
            set => SetValue(IsShownProperty, value);
        }
        protected override void OnAttachedTo(VisualElement bindable)
        {
            _associatedView = bindable;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            _associatedView = null;
            base.OnDetachingFrom(bindable);
        }

        static async void OnDataHandlerBehavior(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DataHandlerBehavior behavior && newValue is bool val)
            {
                foreach (BindableObject bind in behavior.Actions)
                {
                    bind.BindingContext = behavior.BindingContext;
                    var action = (IAction)bind;
                    await action.Execute(behavior, null);
                }
            }
        }
    }
}
