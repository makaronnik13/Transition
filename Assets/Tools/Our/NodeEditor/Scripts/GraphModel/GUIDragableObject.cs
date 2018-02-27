using UnityEngine;
using System.Collections;
using System;

namespace GraphEditor
{
    public class GUIDraggableObject : ScriptableObject
    {
        [SerializeField]
        private Vector2 m_Position;
        private Vector2 m_DragStart;
        private bool m_Dragging;

        public Action<Vector2> onDrag;

        public virtual void Init(Vector2 position)
        {
            m_Position = position;
        }

        public bool Dragging
        {
            get
            {
                return m_Dragging;
            }
        }




        public Vector2 Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        public bool Drag(Rect draggingRect)
        {
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                m_Dragging = false;
            }
            else if (Event.current.type == EventType.MouseDown && draggingRect.Contains(Event.current.mousePosition) && Event.current.button == 0)
            {
                m_Dragging = true;
                m_DragStart = Event.current.mousePosition - m_Position;
                Event.current.Use();
            }

            if (m_Dragging)
            {
                m_Position = Event.current.mousePosition - m_DragStart;
            }

            if (onDrag != null)
            {
                onDrag.Invoke(m_Position);
            }

            return m_Dragging;
        }
    }
}