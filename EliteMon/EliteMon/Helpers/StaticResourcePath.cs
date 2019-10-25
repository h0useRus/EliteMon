using System;
using System.Windows;
using System.Windows.Data;

namespace NSW.EliteDangerous.Monitor.Helpers
{
    public class StaticResourcePath : StaticResourceExtension
    {
        public PropertyPath Path
        {
            get;
            set;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // First get the value from StaticResource
            var o = base.ProvideValue(serviceProvider);
            return (Path == null ? o : PathEvaluator.Evaluate(o, Path));
        }

        private class PathEvaluator : DependencyObject
        {
            /// <summary>
            /// This dummy will hold the end result.
            /// </summary>
            private static readonly DependencyProperty _dummyProperty =
                DependencyProperty.Register("_dummy", typeof(object),
                    typeof(PathEvaluator), new UIPropertyMetadata(null));

            public static object Evaluate(object source, PropertyPath path)
            {
                var d = new PathEvaluator();
                BindingOperations.SetBinding(d, _dummyProperty, new Binding(path.Path) { Source = source });

                // Force binding to give us the desired value defined in path.
                var result = d.GetValue(_dummyProperty);

                // Clear the binding to leave nice memory footprints
                BindingOperations.ClearBinding(d, _dummyProperty);

                return result;
            }
        }
    }

}