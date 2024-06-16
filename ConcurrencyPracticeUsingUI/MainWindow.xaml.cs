using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ConcurrencyPracticeUsingUI
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		private bool enableRecipe62pt2 = false;
		private bool enableRecipe64pt1 = false;
		private bool enableRecipe64pt2 = false;
		private bool enableRecipe65pt2 = false;

		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void btnRecipe62pt1_Click(object sender, RoutedEventArgs e)
		{

			Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

			var uiContext = SynchronizationContext.Current;
			Debug.WriteLine($"UI thread is now {Environment.CurrentManagedThreadId}");

			Observable.Interval(TimeSpan.FromSeconds(1))
				.ObserveOn(uiContext)
				.Subscribe(x => Debug.WriteLine($"Interval {x} on thread {Environment.CurrentManagedThreadId}"));
		}

		private void chkRecipe62pt2_Checked(object sender, RoutedEventArgs e)
		{
			enableRecipe62pt2 = !enableRecipe62pt2;
		}

		private void chkRecipe64pt1_Checked(object sender, RoutedEventArgs e)
		{
			enableRecipe64pt1 = !enableRecipe64pt1;
		}
		private void chkRecipe64pt2_Checked(object sender, RoutedEventArgs e)
		{
			enableRecipe64pt2 = !enableRecipe64pt2;
		}
		private void chkRecipe65pt2_Checked(object sender, RoutedEventArgs e)
		{
			enableRecipe65pt2 = !enableRecipe65pt2;
		}

		/// <summary>
		/// Recipe62pt2 Handles the PointerMoved event of the mainStackPanel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
		private void mainStackPanel_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (enableRecipe62pt2)
			{
				var uiContext = SynchronizationContext.Current;
				Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

				Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainStackPanel.PointerMoved += addHandler,
					deleteHandler => mainStackPanel.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainStackPanel))
					.ObserveOn(uiContext)
					.Select(position =>
					{
						//do complex stuff here
						Thread.Sleep(100);

						var result = position.Position.X + position.Position.Y;
						var thread = Environment.CurrentManagedThreadId;
						Debug.WriteLine($"Calculated result {result} on thread {thread}");
						return result;
					})
					.ObserveOn(uiContext)
					.Subscribe(x => Debug.WriteLine($"Interval {x} on thread {Environment.CurrentManagedThreadId}"));
			}

			if (enableRecipe64pt1)
			{
				var uiContext = SynchronizationContext.Current;
				Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

				Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainWindow.Content.PointerMoved += addHandler,
					deleteHandler => mainWindow.Content.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainWindow.Content))
					.Throttle(TimeSpan.FromSeconds(1))
					.ObserveOn(uiContext)//without this its on the wrong thread
					.Subscribe(x => {
						var thread = Environment.CurrentManagedThreadId;
						Debug.WriteLine($"thread {thread}");
						Debug.WriteLine($"{DateTime.Now.Second}: Saw {x.Position.X + x.Position.Y}"); 
					});

			}

			if (enableRecipe64pt2)
			{
				var uiContext = SynchronizationContext.Current;
				Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

				Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainWindow.Content.PointerMoved += addHandler,
					deleteHandler => mainWindow.Content.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainWindow.Content))
					.Sample(TimeSpan.FromSeconds(1))
					.ObserveOn(uiContext)//without this its on the wrong thread
					.Subscribe(x => {
						var thread = Environment.CurrentManagedThreadId;
						Debug.WriteLine($"thread {thread}");
						Debug.WriteLine($"{DateTime.Now.Second}: Saw {x.Position.X + x.Position.Y}");
					});

			}

			if (enableRecipe65pt2)
			{
				var uiContext = SynchronizationContext.Current;
				Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

				Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainWindow.Content.PointerMoved += addHandler,
					deleteHandler => mainWindow.Content.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainWindow.Content))
					.Timeout(TimeSpan.FromSeconds(1))
					.ObserveOn(uiContext)//without this its on the wrong thread
					.Subscribe(x => {
						var thread = Environment.CurrentManagedThreadId;
						Debug.WriteLine($"thread {thread}");
						Debug.WriteLine($"{DateTime.Now.Second}: Saw {x.Position.X + x.Position.Y}");
					},
					ex => { Trace.WriteLine(ex); });

			}

		}

		private void btnRecipe64pt1_Click(object sender, RoutedEventArgs e)
		{
			Observable.Interval(TimeSpan.FromSeconds(1))
				.Buffer(2)
				.Subscribe(x => Debug.WriteLine($"{DateTime.Now.Second}: Got {x[0]} and {x[1]}"));
		}
		private void btnRecipe64pt2_Click(object sender, RoutedEventArgs e)
		{
			Observable.Interval(TimeSpan.FromSeconds(1))
				.Window(2)
				.Subscribe(group =>
				{
					Debug.WriteLine($"{DateTime.Now.Second}:Starting new group");

					group.Subscribe(x =>
						Debug.WriteLine($"{DateTime.Now.Second}: Saw {x}"),
						() => Debug.WriteLine($"{DateTime.Now.Second}: Ending group")
					);
				});
		}

		private void btnRecipe64pt3_Click(object sender, RoutedEventArgs e)
		{
			Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainStackPanel.PointerMoved += addHandler,
					deleteHandler => mainStackPanel.PointerMoved -= deleteHandler)
				.Buffer(TimeSpan.FromSeconds(1))
				.Subscribe(x => Trace.WriteLine($"{DateTime.Now.Second}: Saw{x.Count} items."));
		}

		private void btnRecipe65pt1_Click(object sender, RoutedEventArgs e)
		{
			using (var client = new HttpClient())
			{
				client.GetStringAsync("https://httpbin.org/get")
					.ToObservable()
					.Timeout(TimeSpan.FromSeconds(1))
					.Subscribe(
					x => Trace.WriteLine($"{DateTime.Now.Second}: Saw {x.Length}"),
					ex => Trace.WriteLine(ex));
			}
		}

		private void btnRecipe65pt3_Click(object sender, RoutedEventArgs e)
		{
			var uiContext = SynchronizationContext.Current;
			Debug.WriteLine($"UI thread is {Environment.CurrentManagedThreadId}");

			var mockedTaskEvents = Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainWindow.Content.PointerMoved += addHandler,
					deleteHandler => mainWindow.Content.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainWindow.Content));

			Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
					conversionHandler => (s, a) => conversionHandler(s, a),
					addHandler => mainWindow.Content.PointerMoved += addHandler,
					deleteHandler => mainWindow.Content.PointerMoved -= deleteHandler)
					.Select(evt => evt.EventArgs.GetCurrentPoint(mainWindow.Content))
					.Timeout(TimeSpan.FromSeconds(1), mockedTaskEvents)
					.ObserveOn(uiContext)//without this its on the wrong thread
					.Subscribe(x => {
						var thread = Environment.CurrentManagedThreadId;
						Debug.WriteLine($"thread {thread}");
						Debug.WriteLine($"{DateTime.Now.Second}: Saw {x.Position.X + x.Position.Y}");
					},
					ex => { Trace.WriteLine(ex); });
		}
	}
}
