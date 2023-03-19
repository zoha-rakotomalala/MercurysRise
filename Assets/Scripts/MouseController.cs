using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;
   

        // Update is called once per frame
        void LateUpdate()
        {
            RaycastHit2D? highlight = HighlightTile();

            if (highlight.HasValue)
            {
                GameObject overlayTile = highlight.Value.collider.gameObject;
                cursor.transform.position = overlayTile.transform.position;

                if (Input.GetMouseButtonDown(0))
                {
                overlayTile.GetComponent<SpriteRenderer>().color = cursor.GetComponent<SpriteRenderer>().color;
                }
            }
        }

        public RaycastHit2D? HighlightTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] highlights = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

            if(highlights.Length > 0)
            {
                return highlights.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
