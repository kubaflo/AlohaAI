using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace AlohaAI.Platforms.iOS.Handlers;

public class TranslucentShellRenderer : ShellRenderer
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
    {
        return new TranslucentTabBarAppearanceTracker();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        ClearBackgrounds();
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        ClearBackgrounds();
    }

    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();
        ClearBackgrounds();
    }

    private void ClearBackgrounds()
    {
        if (View == null) return;
        
        // Make all views in the hierarchy transparent so the window background shows through
        View.BackgroundColor = UIColor.Clear;
        View.Opaque = false;
        
        var current = View.Superview;
        while (current != null && current is not UIWindow)
        {
            current.BackgroundColor = UIColor.Clear;
            current.Opaque = false;
            current = current.Superview;
        }
        
        // Also clear child container views that MAUI adds
        ClearChildBackgrounds(View, depth: 0);
    }
    
    private static void ClearChildBackgrounds(UIView view, int depth)
    {
        if (depth > 3) return;
        
        foreach (var child in view.Subviews)
        {
            if (child is UITabBar) continue;
            
            child.BackgroundColor = UIColor.Clear;
            child.Opaque = false;
            ClearChildBackgrounds(child, depth + 1);
        }
    }
}

public class TranslucentTabBarAppearanceTracker : IShellTabBarAppearanceTracker
{
    public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
    {
        var tabBar = controller.TabBar;
        var barAppearance = new UITabBarAppearance();
        barAppearance.ConfigureWithTransparentBackground();

        // Dark blur letting the sunset background show through
        barAppearance.BackgroundEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.SystemChromeMaterialDark);
        barAppearance.BackgroundColor = UIColor.FromRGBA(30, 20, 52, 200);
        barAppearance.ShadowColor = UIColor.FromRGBA(255, 255, 255, 20);

        tabBar.StandardAppearance = barAppearance;
        tabBar.ScrollEdgeAppearance = barAppearance;
        tabBar.Translucent = true;
        tabBar.BackgroundColor = UIColor.Clear;
        tabBar.BarTintColor = UIColor.Clear;
        tabBar.TintColor = UIColor.FromRGB(232, 139, 191);
        tabBar.UnselectedItemTintColor = UIColor.FromRGB(184, 176, 194);
    }

    public void ResetAppearance(UITabBarController controller)
    {
        var tabBar = controller.TabBar;
        var barAppearance = new UITabBarAppearance();
        barAppearance.ConfigureWithDefaultBackground();
        tabBar.StandardAppearance = barAppearance;
        tabBar.ScrollEdgeAppearance = barAppearance;
    }

    public void UpdateLayout(UITabBarController controller)
    {
        var tabBar = controller.TabBar;
        var rootView = controller.View;
        if (tabBar == null || rootView == null) return;

        var horizontalInset = (nfloat)18;
        var barHeight = (nfloat)68;
        var safeBottom = rootView.SafeAreaInsets.Bottom;
        var width = rootView.Bounds.Width - (horizontalInset * 2);
        var y = rootView.Bounds.Height - barHeight - safeBottom - 8;

        tabBar.Frame = new CGRect(horizontalInset, y, width, barHeight);
        tabBar.Layer.CornerRadius = 24;
        tabBar.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner
                                     | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;
        tabBar.Layer.MasksToBounds = true;
        tabBar.ItemPositioning = UITabBarItemPositioning.Fill;
        tabBar.ItemSpacing = 6;

        if (tabBar.Items == null) return;
        foreach (var item in tabBar.Items)
        {
            item.ImageInsets = new UIEdgeInsets(4, 0, -4, 0);
            item.TitlePositionAdjustment = new UIOffset(0, 2);
        }
    }

    public void Dispose() { }
}
