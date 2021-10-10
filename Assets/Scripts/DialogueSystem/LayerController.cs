using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LayerController : MonoBehaviour
{
    public static LayerController instance;

    public Layer background = new Layer();
    public Layer foreground = new Layer();
    public Layer cinematic = new Layer();

    [Tooltip("The RawImage or CanvasGroup prefab for a new image or movie on a layer.")]

    public GameObject graphicPrefab;

    public static float transitionSpeed = 3f;

    void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class Layer
    {
        public Transform panel;
        [HideInInspector] public GraphicObject currentGraphic = null;
        [HideInInspector] public List<GraphicObject> oldGraphics = new List<GraphicObject>();

        [HideInInspector] public float transitionSpeed = 10f;

        public void SetTexture(Texture2D tex, float transitionSpeed = 1f)
        {
            this.transitionSpeed = transitionSpeed;
            CreateGraphic(tex);
        }

        public void SetVideo(VideoClip clip, float transitionSpeed = 1f, bool useAudio = true)
        {
            this.transitionSpeed = transitionSpeed;
            CreateGraphic(clip, useAudio);
        }

        public void ClearLayer()
        {
            if (currentGraphic != null && !currentGraphic.isNull)
            {
                currentGraphic.Disable();
            }

            foreach (GraphicObject graphic in oldGraphics)
            {
                graphic.Disable();
            }
        }

        void CreateGraphic(Texture2D tex)
        {
            if (currentGraphic != null && !currentGraphic.isNull && currentGraphic.renderer.texture == tex)
            {
                return;
            }

            foreach (GraphicObject graphic in oldGraphics)
            {
                if (graphic.NameMatchesGraphic(tex))
                {
                    if (currentGraphic != null && !currentGraphic.isNull && !oldGraphics.Contains(currentGraphic))
                    {
                        oldGraphics.Add(currentGraphic);
                        currentGraphic.Disable(transitionSpeed);
                    }

                    oldGraphics.Remove(graphic);
                    currentGraphic = graphic;
                    currentGraphic.Enable(transitionSpeed);
                    return;
                }
            }

            //At this point, the graphic does not exist and we need to create it.
            GraphicObject newGraphic = new GraphicObject(this, tex);

            //if there is already a current graphic, set it to be an old one.
            if (currentGraphic != null && !currentGraphic.isNull && !oldGraphics.Contains(currentGraphic))
            {
                oldGraphics.Add(currentGraphic);
                currentGraphic.Disable(transitionSpeed);
            }

            currentGraphic = newGraphic;
            currentGraphic.Enable(transitionSpeed);
        }

        void CreateGraphic(VideoClip clip, bool useAudio = true)
        {
            if (currentGraphic != null && !currentGraphic.isNull && currentGraphic.NameMatchesGraphic(clip))
            {
                return;
            }

            foreach (GraphicObject graphic in oldGraphics)
            {
                if (graphic.NameMatchesGraphic(clip))
                {
                    if (currentGraphic != null && !currentGraphic.isNull && !oldGraphics.Contains(currentGraphic))
                    {
                        oldGraphics.Add(currentGraphic);
                        currentGraphic.Disable(transitionSpeed);
                    }
                    oldGraphics.Remove(graphic);
                    currentGraphic = graphic;
                    currentGraphic.Enable(transitionSpeed);
                    return;
                }

            }

            //At this point, the graphic does not exist and we need to create it.
            GraphicObject newGraphic = new GraphicObject(this, clip, useAudio);

            //if there is already a current graphic, set it to be an old one.
            if (currentGraphic != null && !currentGraphic.isNull && !oldGraphics.Contains(currentGraphic))
            {
                oldGraphics.Add(currentGraphic);
                currentGraphic.Disable(transitionSpeed);
            }

            currentGraphic = newGraphic;
            currentGraphic.Enable(transitionSpeed);
        }

        public class GraphicObject
        {
            public bool isNull { get { return canvasgroup == null; } }

            [HideInInspector] public Layer layer;

            public CanvasGroup canvasgroup;

            public RawImage renderer;

            bool isVideo { get { return video != null; } }

            [HideInInspector] public VideoPlayer video = null;

            public GraphicObject(Layer layer, Texture2D tex)
            {
                this.layer = layer;
                GameObject ob = Object.Instantiate(LayerController.instance.graphicPrefab, LayerController.instance.transform) as GameObject;
                ob.SetActive(true);
                canvasgroup = ob.GetComponent<CanvasGroup>();
                renderer = ob.GetComponent<RawImage>();
                renderer.texture = tex;
                ob.transform.SetParent(layer.panel);

                canvasgroup.name = $"Graphic - [{tex.name}]";
            }

            public GraphicObject(Layer layer, VideoClip clip, bool useVideoAudio = true)
            {
                this.layer = layer;
                GameObject ob = Object.Instantiate(LayerController.instance.graphicPrefab, LayerController.instance.transform) as GameObject;
                ob.SetActive(true);
                canvasgroup = ob.GetComponent<CanvasGroup>();
                renderer = ob.GetComponent<RawImage>();
                ob.transform.SetParent(layer.panel);

                RenderTexture tex = new RenderTexture(Mathf.RoundToInt(clip.width), Mathf.RoundToInt(clip.height), 0);
                renderer.texture = tex;

                video = renderer.gameObject.AddComponent<VideoPlayer>();
                video.source = VideoSource.VideoClip;
                video.clip = clip;
                video.renderMode = VideoRenderMode.RenderTexture;
                video.targetTexture = tex;
                video.isLooping = true;

                video.SetDirectAudioVolume(0, 0);
                if (!useVideoAudio)
                {
                    video.SetDirectAudioMute(0, true);
                }

                canvasgroup.name = $"Graphic - [{clip.name}]";
            }

            public bool NameMatchesGraphic(Texture2D tex)
            {
                return canvasgroup.name == $"Graphic - [{tex.name}]";
            }


            public bool NameMatchesGraphic(VideoClip clip)
            {
                return canvasgroup.name == $"Graphic - [{clip.name}]";
            }

            public void Enable(float speed = 1f)
            {
                layer.transitionSpeed = speed;

                if (!isEnabling)
                {
                    _enabling = true;
                    if (settingVisibility != null)
                    {
                        LayerController.instance.StopCoroutine(settingVisibility);
                    }
                    settingVisibility = LayerController.instance.StartCoroutine(SetVisibility(1f));
                }
            }

            public void Disable(float speed = 1f)
            {
                layer.transitionSpeed = speed;

                if (!isDisabling)
                {
                    _disabling = true;
                    if (settingVisibility != null)
                    {
                        LayerController.instance.StopCoroutine(settingVisibility);
                    }
                    settingVisibility = LayerController.instance.StartCoroutine(SetVisibility(0f));
                }
            }

            bool _enabling = false;
            bool _disabling = false;
            public bool isEnabling { get { return _enabling; } }
            public bool isDisabling { get { return _disabling; } }
            Coroutine settingVisibility = null;

            IEnumerator SetVisibility(float alpha)
            {
                while (canvasgroup.alpha != alpha)
                {
                    canvasgroup.alpha = Mathf.MoveTowards(canvasgroup.alpha, alpha, layer.transitionSpeed * LayerController.transitionSpeed * Time.deltaTime);
                    if (isVideo)
                    {
                        video.SetDirectAudioVolume(0, canvasgroup.alpha);
                    }

                    yield return new WaitForEndOfFrame();
                }

                settingVisibility = null;
                _enabling = false;
                _disabling = false;

                if (alpha == 0)
                {
                    Destroy();
                }
            }

            void Destroy()
            {
                Object.DestroyImmediate(canvasgroup.gameObject);

                if (layer.oldGraphics.Contains(this))
                {
                    layer.oldGraphics.Remove(this);
                }
                else
                {
                    layer.currentGraphic = null;
                }
            }

        }

    }

}
