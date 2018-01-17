# (c) noobtuts.com 2015
# Note: add this Script to the Camera
import UnityEngine;

# map(iseven, [1, 2, 3, 4]) => [False, True, False, True]
def map(fn as ICallable, coll):
    return [fn(x) for x in coll]

# creates a rect around two points
def rect_around(a as Vector2, b as  Vector2) as Rect:
    min = Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y))
    max = Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y))
    return Rect.MinMaxRect(min.x, min.y, max.x, max.y)

# does a rect contain a collider?
def rect_contains3D(cam as Camera, r as Rect, c as Collider):
    # world to screen position
    s = cam.WorldToScreenPoint(c.bounds.center)
    
    # contains it?
    return r.Contains(Vector2(s.x, Screen.height - s.y))   

# is a collider visible or behind a wall?
def is_visible(cam as Camera, c as Collider):
    # world to screen position
    s = cam.WorldToScreenPoint(c.bounds.center)
    
    # screen to world raycast to find out if anything is inbetween
    ray = cam.ScreenPointToRay(s)
    hit = RaycastHit()
    if Physics.Raycast(ray, hit):
        return c == hit.collider

# call 'OnSelect' for a GameObject        
def call_onselect(g as GameObject):
    g.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver)

# call 'OnDeselect' for a GameObject
def call_ondeselect(g as GameObject):
    g.SendMessage("OnDeselect", SendMessageOptions.DontRequireReceiver)

# call 'OnSelect' for multiple GameObjects
def call_onselect_multi(lis as List):
    map({g as GameObject | call_onselect(g)}, lis)
            
# call ondeselect for each GameObject in 'lis', unless its in 'ignr'
def call_ondeselect_multi_except(lis as List, ignr as List):
    for g as GameObject in lis:
        if g and not g in ignr:
            call_ondeselect(g)

[RequireComponent(typeof(Camera))]
class RtsSelectionSystem(MonoBehaviour):
    # mouse button to use
    public mousebutton = 0
    
    # only select objects that aren't behind walls?
    public checkVisibility = false
    
    # allow selection rectangle customization
    public skin as GUISkin
    
    # selection rectangle
    start = Vector2.zero
    cur = Vector2.zero
    visible = false
    
    # last selection
    last as List = []
    
    # cache
    cam as Camera

    def Start():
        cam = GetComponent[of Camera]()
    
    # find the clicked object by raycasting against colliders
    def find_clicked(cam as Camera):
        ray = cam.ScreenPointToRay(Input.mousePosition)
        hit = RaycastHit()
        if Physics.Raycast(ray, hit):
            return hit.transform.gameObject
    
    # find the objects inside the rectangle
    def find_selected(cam as Camera, r as Rect):        
        # find all colliders in scene
        all = List(FindObjectsOfType[of Collider]())
                
        # find the ones that are in-rect & active & visible
        if checkVisibility:
            return [c.gameObject for c as Collider in all if rect_contains3D(cam, r, c) and is_visible(cam, c) and c.gameObject.activeInHierarchy]
        # find the ones that are in-rect & active
        else:
            return [c.gameObject for c as Collider in all if rect_contains3D(cam, r, c) and c.gameObject.activeInHierarchy]
    
    def Update():
        # multi selection started? set rect pos to current mouse pos
        if Input.GetMouseButtonDown(mousebutton):
            start.x = Input.mousePosition.x
            start.y = Screen.height - Input.mousePosition.y
            visible = true
    
        # multi selection in progress? update rect
        if Input.GetMouseButton(mousebutton):
            cur.x = Input.mousePosition.x
            cur.y = Screen.height - Input.mousePosition.y
        
        # selection finished?
        if Input.GetMouseButtonUp(mousebutton):
            # single click or multi selection?
            if start == cur:
                # simple raycasting then
                go = find_clicked(cam)
                if go:
                    # deselect -> select
                    call_ondeselect_multi_except(last, [go])
                    call_onselect(go)
                            
                    # save as last selection
                    last = [go]
            else:
                # find selected objects
                sel = find_selected(cam, rect_around(start, cur))
                
                # deselect -> select
                call_ondeselect_multi_except(last, sel)
                call_onselect_multi(sel)
                
                # save as last selection
                last = sel            
                                
            # don't draw the rectangle anymore
            visible = false
    
    def OnGUI():
        # selection visible and mouse moved a bit? then draw the rectangle
        GUI.skin = skin
        if visible and not cur.Equals(start):
            GUI.Box(rect_around(start, cur), "")
        GUI.skin = null
