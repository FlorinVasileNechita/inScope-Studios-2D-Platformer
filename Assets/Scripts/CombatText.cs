using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {
    private float speed;
    private float fadeTime;
    private Vector3 direction;
    public AnimationClip critAnim;
    private bool crit;
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!crit) {
            float translation = speed * Time.deltaTime;
            transform.Translate(direction * translation);
        }

        transform.LookAt(2 * transform.position - CombatTextManager.Instance.player.transform.position);

    }

    public void Initialize(float speed, Vector3 direction, float fadeTime, bool crit) {
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        this.crit = crit;

        if (crit) {
            GetComponent<Animator>().SetTrigger("Critical");
            StartCoroutine(Critical());
        }
        else {
            StartCoroutine(Fadeout());
        }

    }

    private IEnumerator Critical() {
        yield return new WaitForSeconds(critAnim.length);
        crit = false;
        StartCoroutine(Fadeout());
    }

    private IEnumerator Fadeout() {
        float startAlpha = GetComponent<Text>().color.a;

        float rate = 1.0f / fadeTime;
        float progress = 0.0f;

        while (progress < 1.0f) {
            Color tmpColor = GetComponent<Text>().color;
            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));
            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}