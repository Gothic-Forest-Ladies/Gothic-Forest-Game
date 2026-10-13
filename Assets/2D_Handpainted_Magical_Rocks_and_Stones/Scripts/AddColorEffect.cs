using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalRocksAndStones
{
	public class AddColorEffect : MonoBehaviour
	{
	    private class RendererColorPair
	    {
	        public SpriteRenderer renderer;
	        public Color startColor;
	    }


	    public float glowStrength = 1.0f;
	    public float particlesStrength = 1.0f;

	    public Color runeColor;
	    public Color particlesColor;

	    private Color runeColorLastFrame;
	    private float glowStrengthLastFrame;
	    private Color particlesColorLastFrame;
	    private float particlesStrengthLastFrame;
	    private List<RendererColorPair> rendsColors;
	    private List<ParticleSystemRenderer> partSystems;

	    void Start()
	    {
	        runeColorLastFrame = Color.black;
	        rendsColors = new List<RendererColorPair>();

	        List <SpriteRenderer> spriteRends = this.GetComponentsInChildren<SpriteRenderer>().ToList();
	        foreach (SpriteRenderer rend in spriteRends)
	        {
	            rendsColors.Add(new RendererColorPair() { renderer = rend, startColor = rend.color }); 
	        }

	        partSystems = this.GetComponentsInChildren<ParticleSystemRenderer>().ToList();
	    }

	    void Update()
	    {
	        if ((runeColor != runeColorLastFrame)||(glowStrength!=glowStrengthLastFrame))
	        {
	            foreach (RendererColorPair rendColorPair in rendsColors)
	            {
	                float targetBrightness = rendColorPair.startColor.r + rendColorPair.startColor.g + rendColorPair.startColor.b;
	                Color combinedColor = new Color(rendColorPair.startColor.r + (runeColor.r* glowStrength), rendColorPair.startColor.g + (runeColor.g*glowStrength), rendColorPair.startColor.b + (runeColor.b* glowStrength), rendColorPair.startColor.a);
	                float combinedColorBrightness = combinedColor.r + combinedColor.g + combinedColor.b;
	                float correctionCoef = targetBrightness / combinedColorBrightness;
	                Color correctedColor = new Color(combinedColor.r* correctionCoef, combinedColor.g * correctionCoef, combinedColor.b * correctionCoef);

	                rendColorPair.renderer.color = correctedColor;
	            }
	            runeColorLastFrame = runeColor;
	            glowStrengthLastFrame = glowStrength;
	        }

	        if ((particlesColor != particlesColorLastFrame)||(particlesStrength!=particlesStrengthLastFrame))
	        {
	            foreach (ParticleSystemRenderer partSysRenderer in partSystems)
	            {
	                partSysRenderer.material.color = new Color(particlesColor.r* particlesStrength, particlesColor.g* particlesStrength, particlesColor.b* particlesStrength, particlesColor.a);
	            }
	            particlesColorLastFrame = particlesColor;
	            particlesStrengthLastFrame = particlesStrength;
	        }
	    }
	}
}
