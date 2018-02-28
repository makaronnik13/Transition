using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GraphEditor
{
    public class NodeEditor
    {
        private EditorWindow parentWindow;

        private NodeGraph graph;
        public NodeGraph NodeGraph
        {
            get
            {
                return graph;
            }
        }

        public Action<Node> OnNodeDraw = (Node node) => { };
        public Action<Node> OnStateDoubleClick = (Node node) => { };
        public Action<Path> OnPathDelete = (Path path) => { };
        public Action<Node> OnNodeDelete = (Node node) => { };

        public Func<Node, bool> OnNodeCanBeDeleted = (Node node) => { return true; };
        public Func<Path, bool> OnPathCanBeDeleted = (Path path) => { return true; };

        public Func<Node> OnNodeCreation = () => { return null; };
        public Func<Node, Path> OnPathCreation = (Node node) => { return null; };
        public Func<Node, Color> OnGetNodeColor = (Node node) => { return Color.gray * 0.7f; };
        public Func<Node, Vector2> OnGetSize = (Node node) => { return new Vector2(100, 25); };
        private Vector2 screenDelta = Vector2.zero;
        private static Texture2D backgroundTexture;
        private Vector2 lastMousePosition;

        private void Awake()
        {
            OnNodeCreation = () =>
            {
                return (Node)ScriptableObjectUtility.CreateAsset<Node>(graph);
            };
            OnPathCreation = (Node node) =>
            {
                return (Path)ScriptableObjectUtility.CreateAsset<Path>(graph);
            };
        }

        private static Texture2D BackgroundTexture
        {
            get
            {
                if (backgroundTexture == null)
                {
                    backgroundTexture = (Texture2D)Resources.Load("Sprites/background") as Texture2D;
                    backgroundTexture.wrapMode = TextureWrapMode.Repeat;
                }
                return backgroundTexture;
            }
        }


        public NodeEditor(NodeGraph graph, EditorWindow parentWindow)
        {
            this.parentWindow = parentWindow;
            this.graph = graph;
        }

        public void Draw(Rect rect)
        {
            GUI.DrawTextureWithTexCoords(rect, BackgroundTexture, new Rect(0, 0, rect.width / BackgroundTexture.width, rect.height / BackgroundTexture.height));

            ContextMenuEvent(rect);
            DraggingEvent();

            foreach (Node node in graph.nodes)
            {
                DrawPathes(node);
            }

            parentWindow.BeginWindows();
            foreach (Node node in graph.nodes)
            {
                DrawNode(node);
            }
            parentWindow.EndWindows();
            AfterDrawEvents();
        }

        private void AfterDrawEvents()
        {
            Event currentEvent = Event.current;
            if (currentEvent.button == 0)
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    CancelMakingPath();
                }
            }
        }

        private void ContextMenuEvent(Rect rect)
        {
            Event currentEvent = Event.current;

            if (rect.Contains(currentEvent.mousePosition) && currentEvent.button == 1)
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    CancelMakingPath();

                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("New node"), false, () =>
                    {
                        CreateNewNode(new Vector2(currentEvent.mousePosition.x - screenDelta.x, currentEvent.mousePosition.y - screenDelta.y));
                    });
                    menu.ShowAsContext();
                }

            }

            if (currentEvent.isKey && currentEvent.keyCode == KeyCode.Delete)
            {
                if (Selection.activeObject)
                {
                    if (Selection.activeObject.GetType() == typeof(Node))
                    {
                        DeleteNode((Node)Selection.activeObject);
                        currentEvent.Use();
                    }
                    else if (Selection.activeObject.GetType() == typeof(Path))
                    {
                        DeletePath((Path)Selection.activeObject);
                        currentEvent.Use();
                    }
                }
            }
        }

        private void CancelMakingPath()
        {
            Path emptyPath = EmptyPath();
            if (emptyPath)
            {
                DeletePath(emptyPath);
            }
        }

        private void DraggingEvent()
        {
            Event currentEvent = Event.current;
            if (currentEvent.button == 2)
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    lastMousePosition = currentEvent.mousePosition;
                }

                if (currentEvent.type == EventType.MouseDrag)
                {

                    Vector2 mouseMovementDifference = (currentEvent.mousePosition - lastMousePosition);

                    screenDelta += new Vector2(mouseMovementDifference.x, mouseMovementDifference.y);

                    lastMousePosition = currentEvent.mousePosition;
                    currentEvent.Use();
                }
            }
        }

        private Node CreateNewNode(Vector2 position)
        {
            Node state = OnNodeCreation.Invoke();
            state.Init(position);
            graph.nodes.Add(state);
            parentWindow.Repaint();
            return state;
        }

        private void DrawNode(Node node)
        {
            GUI.backgroundColor = OnGetNodeColor.Invoke(node);

            if (Selection.activeObject == node)
            {
                GUI.backgroundColor = GUI.backgroundColor * 1.7f;
            }

            Rect drawRect = new Rect(node.Position + screenDelta, GetNodeSize(node));



            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));
            GUILayout.BeginVertical();

            OnNodeDraw.Invoke(node);
            GUILayout.EndVertical();

            GUILayout.EndArea();

            NodeClickEvent(drawRect, node);



            DrawNodeButtons(node);
			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;

        }

        private void DrawNodeButtons(Node node)
        {
            Vector2 start = new Vector2(node.Position.x, node.Position.y);
            int i = 0;
            foreach (Path p in node.pathes)
            {
                DrawPathButton(screenDelta + start + Vector2.up * GetNodeSize(node).y + Vector2.right * (5 + i * (GetNodeSize(node).x / node.pathes.Count)), p);
                i++;
            }

        }

        private void NodeClickEvent(Rect drawRect, Node node)
        {
            Event currentEvent = Event.current;

            if (drawRect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseUp)
                {
                    if (currentEvent.button == 0)
                    {
                        Path emptyPath = EmptyPath();
                        if (emptyPath)
                        {
                            emptyPath.End = node;
                        }
                    }
                }

                if (currentEvent.type == EventType.MouseDown)
                {

                    if (currentEvent.button == 0)
                    {
                        Path emptyPath = EmptyPath();
                        if (emptyPath)
                        {

                            emptyPath.End = node;
                        }

                        Selection.activeObject = node;

                        parentWindow.Repaint();

                        if (currentEvent.clickCount == 2)
                        {
                            OnStateDoubleClick.Invoke(node);
                            currentEvent.Use();
                        }

                    }

                    if (currentEvent.button == 1)
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("AddChild"), false, () =>
                        {
                            Node newNode = CreateNewNode(node.Position - Vector2.down * 100);
                            CreatePath(node, newNode);
                        });

                        if (OnNodeCanBeDeleted.Invoke(node))
                        {
                            menu.AddItem(new GUIContent("DeleteNode"), false, () =>
                            {

                                DeleteNode(node);
                                return;
                            });
                        }


                        menu.AddItem(new GUIContent("AddPath"), false, () =>
                        {
                            CreatePath(node, null);
                        });
                        menu.ShowAsContext();
                    }

                    CancelMakingPath();
                }

            }

            if (node.Drag(drawRect))
            {
                parentWindow.Repaint();
            }
        }

        private void DeleteNode(Node node)
        {
            if (OnNodeCanBeDeleted.Invoke(node))
            {
                OnNodeDelete.Invoke(node);

                HashSet<Path> deletingPathes = new HashSet<Path>();

                for (int i = 0; i < node.pathes.Count; i++)
                {
                    DeletePath(node.pathes.ToList()[i]);
                }


                foreach (Node tr in graph.nodes)
                {
                    foreach (Path c in tr.pathes)
                    {
                        if (c.End == node)
                        {
                            deletingPathes.Add(c);
                        }
                    }
                }

                for (int i = 0; i < deletingPathes.Count; i++)
                {
                    DeletePath(deletingPathes.ToList()[i]);
                }

                graph.nodes.Remove(node);
                UnityEngine.Object.DestroyImmediate(node, true);
                AssetDatabase.SaveAssets();
                //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(state.Key));
                parentWindow.Repaint();
            }
        }

        private Path EmptyPath()
        {
            foreach (Node node in graph.nodes)
            {
                foreach (Path path in node.pathes)
                {
                    if (path.End == null)
                    {
                        return path;
                    }
                }
            }
            return null;
        }

        private void DeletePath(Path path)
        {
            if (OnPathCanBeDeleted.Invoke(path))
            {
                OnPathDelete.Invoke(path);
                path.Start.pathes.Remove(path);
                UnityEngine.Object.DestroyImmediate(path, true);
                AssetDatabase.SaveAssets();
                parentWindow.Repaint();
            }
        }

        private Path CreatePath(Node node, Node newNode)
        {
            Path newPath = OnPathCreation.Invoke(node);
            Debug.Log(node);
            newPath.Init(node, newNode);
            parentWindow.Repaint();
            return newPath;
        }

        private Vector2 GetNodeSize(Node node)
        {
            return OnGetSize(node);
        }
        private void DrawPathes(Node node)
        {
            Vector2 start = new Vector2(node.Position.x, node.Position.y);

            int i = 0;

            foreach (Path np in node.pathes)
            {
                Vector2 end;
                if (np.End != null)
                {
                    end = np.End.Position + Vector2.right * 150 / 2 + screenDelta;
                }
                else
                {
                    end = Event.current.mousePosition;
                    parentWindow.Repaint();
                }

                DrawNodeCurve(screenDelta + start + Vector2.up * GetNodeSize(node).y + Vector2.right * (5 + i * (GetNodeSize(node).x / node.pathes.Count)), end, Color.white, 2, true);
                i++;
            }

        }

        private void DrawPathButton(Vector2 center, Path path)
        {
            GUI.backgroundColor = Color.gray * 0.7f;

            if (Selection.activeObject == path)
            {
                GUI.backgroundColor = GUI.backgroundColor * 1.7f;
            }

            Rect rect = new Rect(center.x - 8, center.y - 2, 16, 16);
            DrawButtonEvent(rect, path);
            GUILayout.BeginArea(rect, GUI.skin.GetStyle("Box"));
            GUILayout.EndArea();

            GUI.color = Color.white;

        }

        private void DrawButtonEvent(Rect rect, Path path)
        {
            Event currentEvent = Event.current;

            if (rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    if (currentEvent.button == 0)
                    {
                        Selection.activeObject = path;
                        parentWindow.Repaint();
                    }
                    if (currentEvent.button == 1)
                    {
                        GenericMenu menu = new GenericMenu();
                        if (OnPathCanBeDeleted.Invoke(path))
                        {
                            menu.AddItem(new GUIContent("Delete path"), false, () =>
                        {
                            DeletePath(path);
                            parentWindow.Repaint();
                        });
                        }
                        menu.AddItem(new GUIContent("Set path"), false, () =>
                        {
                            path.End = null;
                            parentWindow.Repaint();
                        });

                        menu.ShowAsContext();
                    }

                    CancelMakingPath();
                }

                if (Selection.activeObject == path && Event.current.type == EventType.MouseDrag)
                {
                    if (currentEvent.button == 0)
                    {
                        path.End = null;
                        parentWindow.Repaint();
                    }
                }
            }
        }

        private void DrawNodeCurve(Vector2 startPos, Vector2 endPos, Color c, float width, bool withArrow = false)
        {
            float force = 1f;
            float distanceY = Mathf.Abs(startPos.y - endPos.y);
            float distanceX = Mathf.Abs(startPos.x - endPos.x);
            Vector3 middlePoint = (startPos + endPos) / 2;

            Vector3 startTan1 = startPos;
            Vector3 endTan2 = endPos;
            Vector3 startTan2 = middlePoint;
            Vector3 endTan1 = middlePoint;

            if (startPos.y > endPos.y)
            {
                startTan1 -= Vector3.down * 150;
                endTan2 -= Vector3.up * 150;
                if (startPos.y > endPos.y)
                {
                    endTan1 += Vector3.up * Mathf.Max(distanceY, 50);
                    startTan2 -= Vector3.up * Mathf.Max(distanceY, 50);
                }
                else
                {
                    endTan1 += Vector3.down * Mathf.Max(distanceY, 50);
                    startTan2 -= Vector3.down * Mathf.Max(distanceY, 50);
                }
            }
            else
            {
                startTan1 -= distanceY * Vector3.down / force / 2;
                endTan2 -= distanceY * Vector3.up / force / 2;
                if (startPos.x > endPos.x)
                {
                    endTan1 += distanceX * Vector3.right / force / 2;
                    startTan2 -= distanceX * Vector3.right / force / 2;
                }
                else
                {
                    endTan1 += distanceX * Vector3.left / force / 2;
                    startTan2 -= distanceX * Vector3.left / force / 2;
                }
            }

            Color shadowCol = new Color(0, 0, 0, 0.06f);

            // Draw a shadow
            for (int i = 0; i < 2; i++)
            {
                Handles.DrawBezier(startPos, middlePoint, startTan1, endTan1, shadowCol, null, (i + 1) * 7 * width);
            }
            Handles.DrawBezier(startPos, middlePoint, startTan1, endTan1, c, null, 3 * width);

            for (int i = 0; i < 2; i++)
            {
                Handles.DrawBezier(middlePoint, endPos, startTan2, endTan2, shadowCol, null, (i + 1) * 7 * width);
            }
            Handles.DrawBezier(middlePoint, endPos, startTan2, endTan2, c, null, 3 * width);

            if (withArrow)
            {
                Handles.DrawAAPolyLine(5, new Vector3[] { endPos + new Vector2(5, -10), endPos, endPos + new Vector2(-5, -10) });
            }
        }

    }
}
