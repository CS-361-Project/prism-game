using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorModel : MonoBehaviour {
	Image r, g, b, c, m, y, w;
	// Use this for initialization
	void Start () {
		r = transform.Find("Red").GetComponent<Image>();
		r.color = CustomColors.Red;
		g = transform.Find("Green").GetComponent<Image>();
		g.color = CustomColors.Green;
		b = transform.Find("Blue").GetComponent<Image>();
		b.color = CustomColors.Blue;
		c = transform.Find("Cyan").GetComponent<Image>();
		c.color = CustomColors.Cyan;
		m = transform.Find("Magenta").GetComponent<Image>();
		m.color = CustomColors.Magenta;
		y = transform.Find("Yellow").GetComponent<Image>();
		y.color = CustomColors.Yellow;
		w = transform.Find("White").GetComponent<Image>();
		w.color = CustomColors.White;
	}
}
