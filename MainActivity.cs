namespace AnimateListViewDelete
{
    using Android.Animation;
    using Android.App;
    using Android.OS;
    using Android.Widget;

    using Java.Lang;

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        /// <summary>
        ///   How long the animation should last, in milliseconds.
        /// </summary>
        private static readonly int AnimationDuration = 1000;
        /// <summary>
        ///   The names of various monkeys.
        /// </summary>
        private static readonly string[] Monkeys =
        {
            "Allen's Swamp Monkey", "Angolan talapoin", "Gabon talapoin", "Patas monkey", "green monkey",
            "grivet", "Bale Mountains vervet", "Tantalus monkey", "vervet monkey", "mabrouck", "Dryas monkey",
            "Diana monkey", "Roloway monkey", "greater spot-nosed monkey", "blue monkey", "silver monkey", "golden monkey",
            "Syke's monkey", "mona monkey", "Campbell's mona monkey", "crested mona monkey", "Wolf's mona monkey",
            "Dent's mona monkey", "lesser spot-nosed monkey", "white-throated guenon", "Sclater's guenon",
            "read-eared guenon", "moustached guenon", "red-tailed monkey", "l'Hoest's monkey", "Preuss's monkey",
            "sun-tailed monkey", "Hamlyn's monkey", "De Brazza's monkey", "Barbary monkey", "lion-tailed macaque",
            "southern pig-tailed macaque", "northern pig-tailed macaque", "Pagai Island macaque", "Siberut macaque",
            "moor macaque", "Tonkean macaque", "booted macaque", "Heck's macaque", "Gorntalo macaque", "Celebes crested macaque",
            "crab-eating macaque", "stump-tailed macaque", "rhesus macaque", "Formosan rock macaque",
            "Japanese macaque", "torque macaque", "bonnet macaque", "Assam macaque", "Tibetan macaque", "Arunachal macaque",
            "grey-cheeked mangabey", "black crested mangabey", "Opdenbosch's mangabey", "Uganda mangabe",
            "Johnston's mangabey", "Osman Hill's mangabey", "kipunji", "Hamadryas baboon", "Guinea baboon", "olive baboon",
            "yellow babboon", "Chacma baboon", "gelada", "sooty mangabey", "collared mangabey", "golden-bellied mangabey",
            "Tana River mangabey", "Sanje mangebey", "mandrill", "drill"
        };
        private MonkeyListAdapter _adapter;
        private ListView _listView;
        private CheckBox _useViewPropertyAnimatorCheckBox;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _listView = FindViewById<ListView>(Resource.Id.listview);
            _useViewPropertyAnimatorCheckBox = FindViewById<CheckBox>(Resource.Id.useViewPropertyAnimatorCheckbox);

            _adapter = new MonkeyListAdapter(this, Monkeys);

            _listView.Adapter = _adapter;
            _listView.ItemClick += HandleItemClick;
        }

        private void HandleItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var monkeyId = e.Id;

            if (_useViewPropertyAnimatorCheckBox.Checked)
            {
                // The ListView won't recycle the view before the ViewPropertyAnimator is finished.
                e.View.Animate().SetDuration(AnimationDuration).Alpha(0)
                    .WithEndAction(new Runnable(() =>{
                        _adapter.Remove(monkeyId);
                        e.View.Alpha = 1f;
                    }));
            }
            else
            {
                var view = e.View;

                // If we don't set this value to true, then the ListView might recycle the view before the animation is done.
                view.HasTransientState = true;

                var animator = ValueAnimator.OfFloat(new[] { 1f, 0f });
                animator.SetDuration(AnimationDuration);
                animator.Update += (o, animatorUpdateEventArgs) => { view.Alpha = (float)animatorUpdateEventArgs.Animation.AnimatedValue; };

                animator.AnimationEnd += delegate{
                    _adapter.Remove(monkeyId);
                    view.Alpha = 1f;
                };
                animator.Start();
            }
        }
    }
}
