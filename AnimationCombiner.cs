using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Configuration;

#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEditor.PackageManager.UI;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;

class AnimationCombiner : EditorWindow
{
    public Animator myAnimator;
    public AnimatorController maincontroller;
    public VRCExpressionParameters mainparam;
    public VRCExpressionsMenu mainmenu;
    
    public List<AnimatorController> controllers = new List<AnimatorController>();
    public List<VRCExpressionsMenu> menus = new List<VRCExpressionsMenu>();
    public List<VRCExpressionParameters> parameters = new List<VRCExpressionParameters>();
    public int controllerNum = 0;
    public int menuNum = 0;
    public int paramNum = 0;

    private Vector2 scrollPos;
    private GUIStyle horizontalLine;

    [MenuItem("Cascadian/AnimationCombiner")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AnimationCombiner window = (AnimationCombiner)EditorWindow.GetWindow(typeof(AnimationCombiner));
        window.Show();
    }
    
    public void OnGUI()
    {
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
        horizontalLine.fixedHeight = 1;
        
        var centeredTextStyle = new GUIStyle("label");
        centeredTextStyle.alignment = TextAnchor.MiddleCenter;
        centeredTextStyle.fontStyle = FontStyle.Bold;
        
        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        if (GUILayout.Button("Auto-Fill with Selected Avatar", GUILayout.Height(30f), GUILayout.Width(Screen.width*0.85f)))
        {
            if (Selection.activeTransform.GetComponent<Animator>() == null) { return; }
            Transform SelectedObj = Selection.activeTransform;
            myAnimator = SelectedObj.GetComponent<Animator>();
            maincontroller = (AnimatorController)SelectedObj.GetComponent<VRCAvatarDescriptor>().baseAnimationLayers[4].animatorController;
            mainparam = SelectedObj.GetComponent<VRCAvatarDescriptor>().expressionParameters;
            mainmenu = SelectedObj.GetComponent<VRCAvatarDescriptor>().expressionsMenu;
        }
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        //Avatar Animator
        GUILayout.Label("AVATAR ANIMATOR", centeredTextStyle);
        myAnimator = (Animator)EditorGUILayout.ObjectField(myAnimator, typeof(Animator), true, GUILayout.Height(40f));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        //FX Animator Controller
        GUILayout.Label("FX AVATAR CONTROLLER", centeredTextStyle);
        maincontroller = (AnimatorController)EditorGUILayout.ObjectField(maincontroller, typeof(AnimatorController), true, GUILayout.Height(40f));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        //VRCExpressionParameters
        GUILayout.Label("EXPRESSION PARAMETERS", centeredTextStyle);
        mainparam = (VRCExpressionParameters)EditorGUILayout.ObjectField(mainparam, typeof(VRCExpressionParameters), true, GUILayout.Height(40f));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        //VRCExpressionMenu
        GUILayout.Label("EXPRESSION MENU", centeredTextStyle);
        mainmenu = (VRCExpressionsMenu)EditorGUILayout.ObjectField(mainmenu, typeof(VRCExpressionsMenu), true, GUILayout.Height(40f));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(20f);
        horizontalLine.fixedHeight = 2;
        HorizontalLine( Color.grey );
        GUILayout.Space(10f);
        
        { // Animation Controller
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Animation Controllers:", EditorStyles.boldLabel);
            GUIStyle customButton = new GUIStyle("button");
            customButton.fontSize = 25;

            if (GUILayout.Button("-", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                if (controllerNum <= 0) {return;}
                controllerNum--;
                controllers.RemoveAt(controllers.Count - 1);
            }

            if (GUILayout.Button("+", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                controllerNum++;
                controllers.Add(null);
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < controllerNum; i++)
            {
                controllers[i] = (AnimatorController)EditorGUILayout.ObjectField(controllers[i], typeof(AnimatorController), true, GUILayout.Height(40f));
            }
        }
        
        GUILayout.Space(10f);
        horizontalLine.fixedHeight = 1;
        HorizontalLine( Color.grey );
        GUILayout.Space(10f);

        { // Expression Parameters
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Expression Parameters:", EditorStyles.boldLabel);
            GUIStyle customButton = new GUIStyle("button");
            customButton.fontSize = 25;

            if (GUILayout.Button("-", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                if (paramNum <= 0) {return;}
                paramNum--;
                parameters.RemoveAt(parameters.Count - 1);
            }

            if (GUILayout.Button("+", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                paramNum++;
                parameters.Add(null);
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < paramNum; i++)
            {
                parameters[i] = (VRCExpressionParameters)EditorGUILayout.ObjectField(parameters[i], typeof(VRCExpressionParameters), true, GUILayout.Height(40f));
            }
        }
        
        GUILayout.Space(10f);
        HorizontalLine( Color.grey );
        GUILayout.Space(10f);

        { // Expression Menu
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Expression Menus:", EditorStyles.boldLabel);
            GUIStyle customButton = new GUIStyle("button");
            customButton.fontSize = 25;

            if (GUILayout.Button("-", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                if (menuNum <= 0) {return;}
                menuNum--;
                menus.RemoveAt(menus.Count - 1);
            }

            if (GUILayout.Button("+", customButton, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                menuNum++;
                menus.Add(null);
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < menuNum; i++)
            {
                menus[i] = (VRCExpressionsMenu)EditorGUILayout.ObjectField(menus[i], typeof(VRCExpressionsMenu), true, GUILayout.Height(40f));
            }
        }
        
        GUILayout.Space(10f);
        horizontalLine.fixedHeight = 2;
        HorizontalLine( Color.grey );
        GUILayout.Space(10f);
        
        EditorGUI.BeginDisabledGroup(!(maincontroller != null && mainmenu != null && mainparam != null));
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        if (GUILayout.Button("Combine!", GUILayout.Height(40f), GUILayout.Width(Screen.width*0.85f)))
        {
            CombineAnimationControllers();
            CombineVRCParameters();
            CombineVRCMenus();
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        GUILayout.Space(20f);

        if (maincontroller != null && mainmenu != null && mainparam != null)
        {
            ConsoleLogUI();
        }
    }

    void CombineAnimationControllers()
    {
        AssetDatabase.SaveAssets();
        foreach (var cont in controllers)
        {
            foreach (var layer in cont.layers)
            {
                if (ContainsLayerName(layer.name)){continue;}
                maincontroller.AddLayer(layer);
            }
            foreach (var param in cont.parameters)
            {
                if (ContainsParamName(param.name)){continue;}
                maincontroller.AddParameter(param);
            }
        }
        AssetDatabase.SaveAssets();
    }

    void CombineVRCParameters()
    {
        foreach (var paramobj in parameters)
        {
            foreach (var param in paramobj.parameters)
            {
                if (ContainsVRCParamName(param.name)){continue;}
                
                VRCExpressionParameters.Parameter[] newList = new VRCExpressionParameters.Parameter[mainparam.parameters.Length + 1];
        
                //Add parameters that were already present
                for (int i = 0; i < mainparam.parameters.Length; i++)
                {
                    newList[i] = mainparam.parameters[i];
                }

                newList[newList.Length - 1] = param;

                mainparam.parameters = newList;
            }
        }
    }

    void CombineVRCMenus()
    {
        foreach (var menuobj in menus)
        {
            if (menuobj == null) return;
            foreach (var menu in menuobj.controls)
            {
                if (ContainsVRCMenuName(menu)){continue;}
                
                mainmenu.controls.Add(menu);
            }
        }
    }
    
    bool ContainsLayerName(string layername)
    {
        foreach (var layer in maincontroller.layers)
        {
            if (layername == layer.name) {return true;}
        }
        return false;
    }
    
    bool ContainsParamName(string paramname)
    {
        foreach (var parname in maincontroller.parameters)
        {
            if (paramname == parname.name) {return true;}
        }
        return false;
    }
    
    bool ContainsVRCParamName(string paramname)
    {
        foreach (var parname in mainparam.parameters)
        {
            if (paramname == parname.name) {return true;}
        }
        return false;
    }
    
    bool ContainsVRCMenuName(VRCExpressionsMenu.Control menuname)
    {
        foreach (var menu in mainmenu.controls)
        {
            if (menuname.name == menu.name && menu.parameter.name == menuname.parameter.name) {return true;}
        }
        return false;
    }
    
    private void ConsoleLogUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos ,  GUILayout.ExpandHeight(true));

        //Check Aniamtor Controller
        foreach (var controlobj in controllers)
        {
            if (controlobj == null) continue;
            
            foreach (var param in controlobj.parameters)
            {
                if (ContainsParamName(param.name))
                {
                    if (param.name.Contains("GestureLeft") || param.name.Contains("GestureLeftWeight") || param.name.Contains("GestureRight") || param.name.Contains("GestureRightWeight")) continue ;
                    EditorGUILayout.HelpBox("Conflicting Controller Parameter: " + param.name, MessageType.Warning);
                }
            }
            
            foreach (var layer in controlobj.layers)
            {
                if (ContainsLayerName(layer.name))
                {
                    if (layer.name.Contains("AllParts") || layer.name.Contains("Left Hand") || layer.name.Contains("Right Hand") || layer.name.Contains("Base Layer")) continue ;
                    EditorGUILayout.HelpBox("Conflicting Controller Layer: " + layer.name, MessageType.Warning);
                }
            }
        }
        
        //Check VRC Parameters
        foreach (var paramobj in parameters)
        {
            if (paramobj == null) continue;
            foreach (var param in paramobj.parameters)
            {
                if (ContainsVRCParamName(param.name))
                {
                    if (param.name.Contains("VRCFaceBlendH") || param.name.Contains("VRCFaceBlendV") || param.name.Contains("VRCEmote")) continue ;
                    EditorGUILayout.HelpBox("Conflicting VRC Parameter: " + param.name, MessageType.Warning);
                }
            }
        }
        
        //Check VRC Menu Controls
        foreach (var menuobj in menus)
        {
            if (menuobj == null) continue;
            foreach (var control in menuobj.controls)
            {
                if (ContainsVRCMenuName(control))
                {
                    EditorGUILayout.HelpBox("Conflicting VRC Menu Control: " + control.name, MessageType.Warning);
                }
            }
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    // utility method
    void HorizontalLine ( Color color ) {
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box( GUIContent.none, horizontalLine );
        GUI.color = c;
    }
    
}
#endif