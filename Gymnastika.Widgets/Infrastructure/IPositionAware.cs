using System.Windows;

namespace Gymnastika.Widgets.Infrastructure
{
    public interface IPositionAware
    {
        Point Position { get; set; }
        int ZIndex { get; set; }
    }
}
