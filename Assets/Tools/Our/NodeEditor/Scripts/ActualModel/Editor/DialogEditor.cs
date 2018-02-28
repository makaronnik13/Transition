using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using GraphEditor;

public class DialogEditor : EditorWindow
{
    private static Dialog editingDialog;
	private static DialogNode editingNode;
	private DialogState editingState
	{
		get{
		if(!editingNode)
		{
				return null;
		}
			return editingNode.dialogState;
		}
	}

    private NodeEditor editingDialogEditor = null;
    private NodeEditor EditingDialogEditor
    {
        get
        {
            if (editingDialogEditor == null || editingDialog!=editingDialogEditor.NodeGraph)
            {
                editingDialogEditor = new NodeEditor(editingDialog, this);            
                editingDialogEditor.OnNodeDraw = (Node node) =>
                {
                    DialogNode dNode = (DialogNode)node;
                    EditorGUILayout.LabelField(node.name);
					//dNode.name = EditorGUILayout.TextArea(dNode.name, GUILayout.Height(EditorGUIUtility.singleLineHeight));
					if (Selection.activeObject == node)
                    {
                        dNode.dialogNodeDescription = EditorGUILayout.TextArea(dNode.dialogNodeDescription, GUILayout.Height(EditorGUIUtility.singleLineHeight*3));
                    }
                };
                editingDialogEditor.OnGetSize = (Node node) =>
                {
                    if (Selection.activeObject == node)
                    {
                        return new Vector2(150, 20+ EditorGUIUtility.singleLineHeight * 3+5);
                    }

                    return new Vector2(150, 20);
                };
                editingDialogEditor.OnNodeCreation = () =>
                {
					DialogNode newNode = (DialogNode)ScriptableObjectUtility.CreateAsset<DialogNode>(editingDialog);
					newNode.dialogState = (DialogState)ScriptableObjectUtility.CreateAsset<DialogState>(editingDialog);


					DialogStateNode startState = (DialogStateNode)ScriptableObjectUtility.CreateAsset<DialogStateNode>(editingDialog);
					startState.Init(new Vector2(position.width/2, position.height/2) + new Vector2(0, -100));
					newNode.dialogState.nodes.Add(startState);
					startState.nodeType = DialogStateNode.StateNodeType.enter;
					startState.name = "Enter state";

					DialogStateNode exitState = (DialogStateNode)ScriptableObjectUtility.CreateAsset<DialogStateNode>(editingDialog);
					exitState.Init(new Vector2(position.width/2, position.height/2) + new Vector2(0, 100));
					newNode.dialogState.nodes.Add(exitState);
					exitState.nodeType = DialogStateNode.StateNodeType.exit;
					exitState.name = "Exit state";

					DialogStateNode middleState = (DialogStateNode)ScriptableObjectUtility.CreateAsset<DialogStateNode>(editingDialog);
					middleState.Init(new Vector2(position.width/2, position.height/2) + new Vector2(0, 0));
					newNode.dialogState.nodes.Add(middleState);

					Path newPath1 = (DialogStatePath)ScriptableObjectUtility.CreateAsset<DialogStatePath>(editingDialog);
					newPath1.Init(startState, middleState);
					((DialogStatePath)newPath1).automatic = true;

					Path newPath2 = (DialogStatePath)ScriptableObjectUtility.CreateAsset<DialogStatePath>(editingDialog);
					newPath2.Init(middleState, exitState);

					return newNode;
                };
                editingDialogEditor.OnPathCreation = (Node node) =>
                {
					DialogPath path = (DialogPath)ScriptableObjectUtility.CreateAsset<DialogPath>(editingDialog);

					DialogStateNode state = (DialogStateNode)ScriptableObjectUtility.CreateAsset<DialogStateNode>(editingDialog);
					state.Init(new Vector2(position.width/2+200, position.height/2+ UnityEngine.Random.Range(-200, 200)));
					((DialogNode)node).dialogState.nodes.Add(state);
					state.nodeType = DialogStateNode.StateNodeType.narrativeExit;
					state.exitPath = path;
					return path;
                };
                editingDialogEditor.OnStateDoubleClick = (Node node) =>
                {
					editingNode = (DialogNode)node;
                };
				editingDialogEditor.OnPathDelete = (Path path) => 
				{
					for(int j = ((DialogNode)path.Start).dialogState.nodes.Count-1; j>=0; j--)
					{

                        Node node = ((DialogNode)path.Start).dialogState.nodes[j];

                        if (((DialogStateNode)node).exitPath == path)
						{
                            HashSet<Path> deletingPathes = new HashSet<Path>();

    
                            foreach (Node tr in ((DialogNode)path.Start).dialogState.nodes)
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
                                DeleteDialogPath(deletingPathes.ToList()[i]);
                            }

                            ((DialogNode)path.Start).dialogState.nodes.Remove(node);
                            UnityEngine.Object.DestroyImmediate(node, true);
                            AssetDatabase.SaveAssets();
                        }	
					}
				};
				editingDialogEditor.OnNodeDelete = (Node node) => 
				{
					DialogNode dNode = (DialogNode)node;
					for(int j = dNode.dialogState.nodes.Count-1; j>=0 ; j--)
					{
						Node insideNode = dNode.dialogState.nodes[j];
						HashSet<Path> deletingPathes = new HashSet<Path>();

						for (int i = 0; i < insideNode.pathes.Count; i++)
						{
							DeleteDialogPath(insideNode.pathes.ToList()[i]);
						}


						foreach (Node tr in dNode.dialogState.nodes)
						{
							foreach (Path c in tr.pathes)
							{
								if (c.End == insideNode)
								{
									deletingPathes.Add(c);
								}
							}
						}

						for (int i = 0; i < deletingPathes.Count; i++)
						{
							DeleteDialogPath(deletingPathes.ToList()[i]);
						}

						dNode.dialogState.nodes.Remove(insideNode);
						UnityEngine.Object.DestroyImmediate(insideNode, true);
					}

					UnityEngine.Object.DestroyImmediate(dNode.dialogState, true);

					AssetDatabase.SaveAssets();
				};
            }
            return editingDialogEditor;
        }
    }

    private NodeEditor editingStateEditor = null;
    private NodeEditor EditingStateEditor
    {
        get
        {
            if (editingStateEditor == null || editingState != editingStateEditor.NodeGraph)
            {
                editingStateEditor = new NodeEditor(editingState, this);

                editingStateEditor.OnNodeDraw = (Node node) =>
                {
					DialogStateNode dNode = (DialogStateNode)node;
					if(dNode.nodeType == DialogStateNode.StateNodeType.narrativeExit)
					{
						dNode.name = "To "+dNode.exitPath.End.name;
					}
                    EditorGUILayout.LabelField(node.name);
					if (Selection.activeObject == node && dNode.nodeType == DialogStateNode.StateNodeType.simple)
                    {
                        dNode.text = EditorGUILayout.TextField(dNode.text, GUILayout.Height(EditorGUIUtility.singleLineHeight * 3));
                    }
                };

                editingStateEditor.OnGetSize = (Node node) =>
                {
					if (Selection.activeObject == node && ((DialogStateNode)node).nodeType == DialogStateNode.StateNodeType.simple)
                    {
                        return new Vector2(150, 20 + EditorGUIUtility.singleLineHeight * 3 + 5);
                    }

                    return new Vector2(150, 20);
                };

                editingStateEditor.OnNodeCreation = () =>
                {
					return (DialogStateNode)ScriptableObjectUtility.CreateAsset<DialogStateNode>(editingDialog);
                };

                editingStateEditor.OnPathCreation = (Node node) =>
                {
					return (DialogStatePath)ScriptableObjectUtility.CreateAsset<DialogStatePath>(editingDialog);
                };

				editingStateEditor.OnGetNodeColor = (Node node) => {
					DialogStateNode dsNode = (DialogStateNode)node;
					switch(dsNode.nodeType)
					{
					case DialogStateNode.StateNodeType.enter:
						return Color.green * 0.7f;
					case DialogStateNode.StateNodeType.simple:
						return Color.gray * 0.7f;
					case DialogStateNode.StateNodeType.exit:
						return Color.red * 0.7f;
					case DialogStateNode.StateNodeType.narrativeExit:
						return Color.magenta * 0.7f;
					}

					return Color.gray * 0.7f;
				};

                editingStateEditor.OnNodeCanBeDeleted = (Node node) =>
                {
                    return ((DialogStateNode)node).nodeType == DialogStateNode.StateNodeType.simple;
                };
            }
            return editingStateEditor;
        }
    }

    [MenuItem("Window/TestWindow")]
    public static DialogEditor Init(NodeGraph graph)
    {
        editingDialog = (Dialog)graph;
        DialogEditor window = (DialogEditor)EditorWindow.GetWindow<DialogEditor>("Node editor", true, new Type[3] {
            typeof(Animator),
            typeof(Console),
            typeof(SceneView)
        });

        window.minSize = new Vector2(600, 400);
        window.ShowAuxWindow();
        return window;
    }

    private void OnGUI()
    {
        if (editingDialog)
        {
            if (editingState)
            {
				EditingStateEditor.Draw(new Rect(new Vector2(200, 20), position.size-Vector2.up*EditorGUIUtility.singleLineHeight*2));
            }
            else
            {
				EditingDialogEditor.Draw(new Rect(new Vector2(200, 20), position.size-Vector2.up*EditorGUIUtility.singleLineHeight*2));
            }

			GUILayout.BeginArea (new Rect(new Vector2(200, 0), new Vector2(position.width, EditorGUIUtility.singleLineHeight*2)));
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("base layer", GUILayout.Width(100)))
			{
				editingNode = null;
			}
			if(editingState)
			{
				GUILayout.Button (editingNode.name, GUILayout.Width(100));
			}


			EditorGUILayout.EndHorizontal ();
			GUILayout.EndArea ();
        }       

		ParametersEditor.Draw (new Rect(Vector2.zero, new Vector2(200, position.height)), editingDialog);
    }

    private void OnDisable()
    {
        if (editingDialog)
        {
            EditorUtility.SetDirty(editingDialog);
        }
    }

	private void DeleteDialogPath(Path path){
		path.Start.pathes.Remove(path);
		UnityEngine.Object.DestroyImmediate(path, true);
		AssetDatabase.SaveAssets();
	}
}
