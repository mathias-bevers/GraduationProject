using UnityEditor;
using UnityEngine;

namespace Delirium.Editor
{
	public class QuickTools : EditorWindow
	{
		private Color color;
		private string emptyName;
		private string groupName;

		private void OnGUI()
		{
			#region Group Items
			EditorGUILayout.BeginHorizontal();

			groupName = EditorGUILayout.TextField("Group Name", groupName);

			if (GUILayout.Button("Group"))
			{
				var parentObject = new GameObject(groupName);

				if (Selection.activeTransform.parent != null) { parentObject.transform.SetParent(Selection.activeTransform.parent); }

				foreach (GameObject gameObject in Selection.gameObjects) { gameObject.transform.SetParent(parentObject.transform); }

				groupName = string.Empty;
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Apply color.
			EditorGUILayout.BeginHorizontal();

			color = EditorGUILayout.ColorField("Color", color);

			if (GUILayout.Button("Apply"))
			{
				foreach (GameObject gameObject in Selection.gameObjects)
				{
					Renderer renderer = gameObject.GetComponent<MeshRenderer>();

					if (renderer != null) { renderer.material.color = color; }
				}
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Create empty
			EditorGUILayout.BeginHorizontal();

			emptyName = EditorGUILayout.TextField("Empty name", emptyName);

			if (GUILayout.Button("Create"))
			{
				var gameObject = new GameObject(emptyName);

				if (Selection.activeTransform != null) { gameObject.transform.SetParent(Selection.activeTransform); }

				emptyName = string.Empty;
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Remove components
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Remove Components") && Selection.activeTransform != null)
			{
				foreach (Component component in Selection.activeTransform.GetComponents<Component>())
				{
					if (component.GetType() == typeof(Transform)) { continue; }

					DestroyImmediate(component);
				}
			}

			EditorGUILayout.EndHorizontal();
			#endregion

			#region Number objects
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Number selected objects"))
			{
				GameObject[] gameObjects = Selection.gameObjects;
				for (var i = 0; i < gameObjects.Length; i++)
				{
					GameObject gameObject = gameObjects[i];

					gameObject.name = $"{gameObjects[i].name}_{i}";
					gameObject.transform.SetAsLastSibling();
				}
			}

			EditorGUILayout.EndHorizontal();
			#endregion
		}

		[MenuItem("Window/Quick tools")]
		private static void ShowWindow()
		{
			var window = GetWindow<QuickTools>();
			window.titleContent = new GUIContent("Quick tools");
			window.Show();
		}
	}
}