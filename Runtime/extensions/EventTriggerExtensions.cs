using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Wowsome {
  public static class EventTriggerExt {
    public static void AddEventTriggerListener(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> callback) {
      //check whether entry exists in trigger already
      EventTrigger.Entry entry = trigger.triggers.Find(x => x.eventID == eventType);
      //create new if not exist yet and add to the trigger
      if (null == entry) {
        entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
      }
      //if exist remove the listener first and add it back to avoid duplicate
      else {
        entry.callback.RemoveListener(callback);
        entry.callback.AddListener(callback);
      }
    }
  }
}