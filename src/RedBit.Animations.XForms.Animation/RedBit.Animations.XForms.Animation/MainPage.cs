using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

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
							ChangeBackgroundColor();
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
		private async void AnimatePanel(){

			// swap the state
			this.PanelShowing = !PanelShowing;

			// show or hide the panel
			if (this.PanelShowing) {
				// hide all children
				foreach (var child in _panel.Children) {
					child.Scale = 0;
				}

				// layout the panel to slide out
				var rect = new Rectangle(_layout.Width - _panel.Width, _panel.Y, _panel.Width, _panel.Height);
				await this._panel.LayoutTo (rect, 250, Easing.CubicIn);

				// scale in the children for the panel
				foreach (var child in _panel.Children) {
					await child.ScaleTo (1.2, 50, Easing.CubicIn);
					await child.ScaleTo (1, 50, Easing.CubicOut);
				}
			} else {
				

				// layout the panel to slide in
				var rect = new Rectangle(_layout.Width, _panel.Y, _panel.Width, _panel.Height);
				await this._panel.LayoutTo (rect, 200, Easing.CubicOut);

				// hide all children
				foreach (var child in _panel.Children) {
					child.Scale = 0;
				}
			}
		}

		/// <summary>
		/// Changes the background color of the relative layout
		/// </summary>
		private void ChangeBackgroundColor(){
			var repeatCount = 0;
			this._layout.Animate (
				// set the name of the animation
				name: "changeBG", 

				// create the animation object and callback
				animation: new Xamarin.Forms.Animation((val) => {
					// val will be a from 0 - 1 and can use that to set a BG color
					if (repeatCount == 0)
						this._layout.BackgroundColor = Color.FromRgb (1 - val, 1 - val, 1 - val);
					else
						this._layout.BackgroundColor = Color.FromRgb (val, val, val);
				}), 

				// set the length
				length: 750,

				// set the repeat action to update the repeatCount
				finished: (val, b) => {
					repeatCount++;
				},

				// determine if we should repeat
				repeat: () => {
					return repeatCount < 1;
				}
			);
		}
    }
}
