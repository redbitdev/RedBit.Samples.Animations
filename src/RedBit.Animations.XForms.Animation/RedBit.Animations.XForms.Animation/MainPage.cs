using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RedBit.Animations.XForms.Animation
{
    public class MainPage : ContentPage
    {
        private RelativeLayout _layout;
        private StackLayout _panel;
        public MainPage()
        {
            // create the layout
            _layout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            CreateButton();
            CreatePanel();

            // set the content
            this.Content = _layout;
        }

        private void CreateButton()
        {
            // create the button
            _layout.Children.Add(
				new AnimatedButton("Show Panel", AnimatePanel)
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

		private double _panelWidth = -1;
		/// <summary>
		/// Creates the right side menu panel
		/// </summary>
        private void CreatePanel()
        {
			if (_panel == null) {
				_panel = new StackLayout {
					Children = {
						new Label {
							Text = "Options",
							HorizontalOptions = LayoutOptions.Start,
							VerticalOptions = LayoutOptions.Start,
							XAlign = TextAlignment.Center,
							TextColor = Color.White
						},
						new AnimatedButton ("Option 1", () => {
							AnimatePanel();
						}),
						new AnimatedButton ("Option 2", () => {
							AnimatePanel();
						}),
						new AnimatedButton ("Option 3", () => {
							AnimatePanel();
						}),
					},
					Padding = 15,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					BackgroundColor = Color.FromRgba (0, 0, 0, 180)
				};

				// add to layout
				_layout.Children.Add (_panel,
					Constraint.RelativeToParent ((p) => {
						return _layout.Width - (this.PanelShowing ? _panelWidth : 0);
					}),
					Constraint.RelativeToParent ((p) => {
						return 0;
					}),
					Constraint.RelativeToParent ((p) => {
						if(_panelWidth == -1)
							_panelWidth = p.Width /3;
						return _panelWidth;
					}),
					Constraint.RelativeToParent((p)=> {
						return p.Height;
					})
				);
			}
        }

		private bool _PanelShowing = false;
		/// <summary>
		/// Gets a value to determine if the panel is showing or not
		/// </summary>
		/// <value><c>true</c> if panel showing; otherwise, <c>false</c>.</value>
		private bool PanelShowing{
			get{
				return _PanelShowing;
			}
			set{
				_PanelShowing = value;
			}
		}

		/// <summary>
		/// Animates the panel in our out depending on the state
		/// </summary>
		private void AnimatePanel(){

			// swap the state
			this.PanelShowing = !PanelShowing;

			// show or hide the panel
			if (this.PanelShowing) {
				var rect = new Rectangle(_layout.Width - _panel.Width, _panel.Y, _panel.Width, _panel.Height);
				this._panel.LayoutTo (rect, 250, Easing.CubicIn);
			} else {
				var rect = new Rectangle(_layout.Width, _panel.Y, _panel.Width, _panel.Height);
				this._panel.LayoutTo (rect, 200, Easing.CubicOut);
			}

		}
    }
}
