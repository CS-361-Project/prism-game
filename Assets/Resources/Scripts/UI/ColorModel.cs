using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorModel : MonoBehaviour {
	Image r, g, b, c, m, y, w;

	public enum colors{red, green, blue, cyan, magenta, yellow, white}

	Image[] colorArray;
	Color[] oldColors;

	// Use this for initialization
	void Awake () {
		colorArray = new Image[Enum.GetNames(typeof(colors)).Length];
		oldColors = new Color[Enum.GetNames(typeof(colors)).Length];
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

	static int indexOf(Color c) {
		int result = -1;
		for (int i = 0; i < CustomColors.colors.Length; i++) {
			if (CustomColors.colors[i] == c) {
				result = i;
				break;
			}
		}
		return result;
	}

	public void onSwitch(Color background){
		int a = indexOf (background);
		resetModel ();
		switch (a) {
		case 0:
			resetModel ();
			break;
		case 1: 
			highlight ((int)colors.red);
			break;
		case 2:
			highlight ((int)colors.green);
			break;
		case 3:
			highlight ((int)colors.red);
			highlight ((int)colors.green);
			highlight ((int)colors.yellow);
			break;
		case 4:
			highlight ((int)colors.blue);
			break;
		case 5:
			highlight ((int)colors.blue);
			highlight ((int)colors.red);
			highlight ((int)colors.magenta);
			break;
		case 6:
			highlight ((int)colors.blue);
			highlight ((int)colors.green);
			highlight ((int)colors.cyan);
			break;
		case 7:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				highlight (i);
			}
			break;
		}
			
	}

	public void resetModel(){
		for (int i = 0; i < colorArray.Length - 1; i++) {
			colorArray [i].color = oldColors [i];
		}
	}

	public void highlight(int color){
		if (colorArray [color].color == CustomColors.Red) {
			colorArray [color].color = new Color (1F, 0, 0);
		}
		if (colorArray [color].color == CustomColors.Green) {
			colorArray [color].color = new Color (0, 1F, 0);
		}
		if (colorArray [color].color == CustomColors.Blue) {
			colorArray [color].color = new Color (0, 0, 1F);
		}
		if (colorArray [color].color == CustomColors.Cyan) {
			colorArray [color].color = new Color (0, 1F, 1F);
		}
		if (colorArray [color].color == CustomColors.Yellow) {
			colorArray [color].color = new Color (1F, 1F, 0);
		}
		if (colorArray [color].color == CustomColors.Magenta) {
			colorArray [color].color = new Color (1F, 0, 1F);
		}
		if (colorArray [color].color == CustomColors.White) {
			colorArray [color].color = new Color (1F, 1F, 1F);
		}
	}

	public void unHighlight(int color){
		colorArray[color].color = oldColors[color];
	}
}
