/************************************************************************************

Copyright   :   Copyright 2015-2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine.EventSystems;

namespace dpn
{
	/// Interface to implement if you wish to receive OnDpnPointerHover callbacks.
	/// Executed by GazeInputModule.cs.
	public interface IDpnPointerHoverHandler : IEventSystemHandler
	{

		/// Called when pointer is hovering over GameObject.
		void OnDpnPointerHover(PointerEventData eventData);
	}
}