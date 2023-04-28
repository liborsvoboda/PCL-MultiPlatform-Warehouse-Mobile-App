using System;
using Xamarin.Forms;
using System.Linq;

namespace Terminal
{

    public class ExtendedLabel : Label
    {
        private event EventHandler click;

        public string Name
        {
            get; set;
        }

        public void DoClick()
        {
            click?.Invoke(this, null);
        }

        public event EventHandler Clicked
        {
            add
            {
                lock (this)
                {
                    click += value;

                    var g = new TapGestureRecognizer();

                    g.Tapped += (s, e) => click?.Invoke(s, e);

                    GestureRecognizers.Add(g);
                }
            }
            remove
            {
                lock (this)
                {
                    click -= value;

                    GestureRecognizers.Clear();
                }
            }
        }
    }

    public class NumericValidationBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
            }
        }


    }
}