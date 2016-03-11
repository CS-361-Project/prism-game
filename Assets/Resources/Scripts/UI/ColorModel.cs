using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ColorModel : MonoBehaviour {
	Image r, g, b, c, m, y, w;

	public enum colors{red, green, blue, cyan, magenta, yellow, white}

	Image[] colorArray = new Image[Enum.GetNames(typeof(colors)).Length];
	Color[] oldColors = new Color[Enum.GetNames(typeof(colors)).Length];
	// Use this for initialization
	void Start () {
		r = transform.Find("Red").GetComponent<Image>();
		r.color = CustomColors.Red;
		colorArray [(int)colors.red] = r;
		oldColors [(int)colors.red] = CustomColors.Red;

		g = transform.Find("Green").GetComponent<Image>();
		g.color = CustomColors.Green;
		colorArray [(int)colors.green] = g;
		oldColors [(int)colors.green] = CustomColors.Green;

		b = transform.Find("Blue").GetComponent<Image>();
		b.color = CustomColors.Blue;
		colorArray [(int)colors.blue] = b;
		oldColors [(int)colors.blue] = CustomColors.Blue;

		c = transform.Find("Cyan").GetComponent<Image>();
		c.color = CustomColors.Cyan;
		colorArray [(int)colors.cyan] = c;
		oldColors [(int)colors.cyan] = CustomColors.Cyan;

		m = transform.Find("Magenta").GetComponent<Image>();
		m.color = CustomColors.Magenta;
		colorArray [(int)colors.magenta] = m;
		oldColors [(int)colors.magenta] = CustomColors.Magenta;

		y = transform.Find("Yellow").GetComponent<Image>();
		y.color = CustomColors.Yellow;
		colorArray [(int)colors.yellow] = y;
		oldColors [(int)colors.yellow] = CustomColors.Yellow;

		w = transform.Find("White").GetComponent<Image>();
		w.color = CustomColors.White;
		colorArray [(int)colors.white] = w;
		oldColors [(int)colors.white] = CustomColors.White;
	}

	public void switchToggled(Color c){
		for (int i = 0; i < colorArray.Length - 1; i++) {
			if (colorArray [i].color == c) {
				highlight (i);
			}
		}
	}

	public void switchUntoggled(Color c){
		for (int i = 0; i < colorArray.Length - 1; i++) {
			if (colorArray [i].color == c) {
				unHighlight (i);
			}
		}
	}

	public void highlight(int color){
		//colorArray [color].color = new Color (colorArray [color].color.r + .1F, colorArray [color].color.g +.1F, colorArray [color].color.b + .1F);
		colorArray[color].color = CustomColors.Black;
	
	}

	public void unHighlight(int color){
		//colorArray [color].color = new Color (colorArray [color].color.r - .1F, colorArray [color].color.g - .1F, colorArray [color].color.b - .1F);
		colorArray[color].color = oldColors[color];
	}
}
