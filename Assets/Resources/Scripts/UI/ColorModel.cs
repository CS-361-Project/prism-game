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

	public bool bgTransitioning = false;
	Color newBG, oldBG;
	float lastColorChange = -.1f;
	public float transitionTime = .1f;
	int transitionIndex;

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

	void Update(){
		if (bgTransitioning) {
			whileBGTransitioning(timeSinceLastColorChange() / transitionTime);
		}
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
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.red) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 2:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.green) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 3:
			
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.red || i == (int)colors.green || i == (int)colors.yellow) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 4:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.blue) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 5:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.red || i == (int)colors.blue || i == (int)colors.magenta) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 6:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				if (i == (int)colors.green || i == (int)colors.blue || i == (int)colors.cyan) {
					newBG = oldColors [i];
					oldBG = oldColors [i];
					highlight (i);
				} else {
					unHighlight (i);
				}
			}
			break;
		case 7:
			for (int i = 0; i < colorArray.Length - 1; i++) {
				newBG = oldColors [i];
				oldBG = oldColors [i];
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
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (1F, 0, 0));
			//colorArray [color].color = new Color (1F, 0, 0);
		}
		if (colorArray [color].color == CustomColors.Green) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (0, 1F, 0));
		}
		if (colorArray [color].color == CustomColors.Blue) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (0, 0, 1F));
			//colorArray [color].color = new Color (0, 0, 1F);
		}
		if (colorArray [color].color == CustomColors.Cyan) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (0, 1F, 1F));
			//colorArray [color].color = new Color (0, 1F, 1F);
		}
		if (colorArray [color].color == CustomColors.Yellow) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (1F, 1F, 0));
			//colorArray [color].color = new Color (1F, 1F, 0);
		}
		if (colorArray [color].color == CustomColors.Magenta) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (1F, 0, 1F));
			//colorArray [color].color = new Color (1F, 0, 1F);
		}
		if (colorArray [color].color == CustomColors.White) {
			transitionIndex = color;
			if (bgTransitioning) {
				finishBGTransitionImmediate();
			}
			startBGTransition(new Color (1F, 1F, 1F));
			//colorArray [color].color = new Color (1F, 1F, 1F);
		}
	}

	public void unHighlight(int color){
		/*transitionIndex = color;
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(Color.Lerp(oldColors[color], new Color (1F, 1F, 1F), .5F));*/

		colorArray[color].color = Color.Lerp(oldColors[color], new Color (1F, 1F, 1F), .5F);
	}

	public void startBGTransition(Color bgColor) {
		newBG = bgColor;
		bgTransitioning = true;
		lastColorChange = Time.time;
	}

	public void finishBGTransitionImmediate() {
		newBG = colorArray[transitionIndex].color;
		whileBGTransitioning(1.0f);
	}

	public void whileBGTransitioning(float t) {
		if (t >= 1) {
			colorArray[transitionIndex].color = newBG;
			oldBG = newBG;
			bgTransitioning = false;
		}
		colorArray[transitionIndex].color = Color.Lerp(oldBG, newBG, t);

	}
	public float timeSinceLastColorChange() {
		return Time.time - lastColorChange;
	}
}
