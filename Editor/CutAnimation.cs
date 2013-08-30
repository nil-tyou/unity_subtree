using UnityEngine;
using UnityEditor;
using System.Collections;


public class CutAnimation : EditorWindow {

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if (Selection.activeGameObject.animation != null) {
			
			string[] clip_name = new string[Selection.activeGameObject.animation.GetClipCount()];
			int i=0;
		
			foreach (AnimationState anim_stat in Selection.activeGameObject.animation){
				clip_name[i++] = anim_stat.clip.name;
			}
			int old_index = selected_index;
			selected_index = EditorGUI.Popup(new Rect(0, 0, 150, 20), selected_index, clip_name);
			
			if (old_index != selected_index) {
				end_frame = (int)(Selection.activeGameObject.animation[clip_name[selected_index]].clip.length * 
					Selection.activeGameObject.animation[clip_name[selected_index]].clip.frameRate);
			}
			
			EditorGUI.LabelField(new Rect(0, 20, 150, 20), "fps:"+((int)Selection.activeGameObject.animation[clip_name[selected_index]].clip.frameRate).ToString());
			EditorGUI.LabelField(new Rect(0, 40, 50, 20), "Start");
			start_frame = EditorGUI.IntField(new Rect(50, 40, 150, 20), start_frame);
			EditorGUI.LabelField(new Rect(0, 60, 50, 20), "End");
			end_frame = EditorGUI.IntField(new Rect(50, 60, 150, 20), end_frame);
			
			new_clip_name = EditorGUI.TextField(new Rect(0, 80, 200, 20), new_clip_name);
			
			if (EditorGUI.Toggle (new Rect(0, 100, 200, 20), "Cut", false)) {
				AnimationClip ref_clip = Selection.activeGameObject.animation[clip_name[selected_index]].clip;
				Selection.activeGameObject.animation.AddClip(ref_clip, new_clip_name, start_frame, end_frame);
				
				AssetDatabase.CreateAsset(Selection.activeGameObject.animation[new_clip_name].clip, "Assets/Animation/"+new_clip_name+".anim");
				AssetDatabase.SaveAssets();
			}
		}
	}
	
	[MenuItem("Window/Cut Animation")]
	static void CutAnim ()
	{
		CutAnimation ca = GetWindow<CutAnimation>();
		ca.Show();
	}
	
	int selected_index = 0;
	int start_frame = 0;
	int end_frame = 0;
	string new_clip_name = "cut_anim_new";
}
