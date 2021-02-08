using System;
using UnityEngine;

namespace MobaGame.Framework.Core
{
    public interface IDynamicModifier : IModifier
	{
        event EventHandler Changed;
	}
    public interface IModifier
    {
	    IModifierView View { get; }
    }

    public class ModifierView : IModifierView
    {
	    private Sprite _icon;

	    public Sprite Icon
	    {
		    get => _icon;
		    set
		    {
			    var changed = (_icon == value);
			    _icon = value;
			    if (changed) 
				    OnChanged();
		    }
	    }
	    public event EventHandler Changed;
	    private void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
    public interface IModifierView : IView
    {
	    Sprite Icon { get; }
	    
    }
	public interface IView
    {
	    /// <summary>
	    /// An event fired when the view is changed.
	    /// </summary>
	    event EventHandler Changed;
    }
}