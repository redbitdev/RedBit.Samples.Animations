using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RedBit.Animations.XForms.Animation
{
    public class MainPage : ContentPage
    {
        private RelativeLayout _layout;
        public MainPage()
        {
            // create the layout
            _layout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            CreateButton();

            // set the content
            this.Content = _layout;
        }

        private void CreateButton()
        {
            // create the button
            _layout.Children.Add(
                    new AnimatedButton("Show Panel")
                    {
                        BackgroundColor = Color.Red,
                        TextColor = Color.White,
                        Padding = 20,
                    },
                    Constraint.RelativeToParent((p) =>
                    {
                        return 10;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return Device.OnPlatform<int>(28, 0, 0);
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return p.Width - (10 * 2);
                    })
                );
        }
    }
}
