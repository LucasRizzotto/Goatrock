Shader "vladstorm/Room" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		

		//Interactions 
		[Space][Header(Ripple)]
		_mdpEdgeTickness("_mdpEdgeTickness", Float) = .1
		_mdpSpeed("_mdpSpeed", Float) = 1.
		_mdpSpeedColor("_mdpSpeedColor", Float) = 1.
		_mdpAmt("_mdpAmt", Float) = 1.
		
		// _mdpRippleColor("_mdpRippleColor", Color) = (1,1,1,1)
		_mdpRippleEmission("_mdpRippleEmission", Float) = 1

		[Space][Header(Resonate)]
		// _mdpMicVolume("_mdpMicVolume", Range(0,1)) = 1
		_mdpMicPitch("_mdpMicPitch", Range(0,1)) = 1
		_mdpResAmt("_mdpResAmt", Vector) = (1,1,1,1)
		_mdpResSpeed("_mdpResSpeed", Vector) = (1,1,1,1)
		_mdpResScale("_mdpResScale", Vector) = (1,1,1,1)
		_mdpResColorLow("_mdpResColorLow", Color) = (1,1,1,1)
		_mdpResColorHigh("_mdpResColorHigh", Color) = (1,1,1,1)
		// _Alpha("_Alpha", Float) = 1


	}
	SubShader {

		Tags { "RenderType"="Transparent" }
		// Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		// #pragma surface surf Standard fullforwardshadows
		#pragma surface surf Standard  vertex:vert  fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		#include "UnityCG.cginc"

		sampler2D _MainTex;


      struct appdata {
         float4 vertex : POSITION;
         float4 tangent : TANGENT;
         float3 normal : NORMAL;
         float2 texcoord : TEXCOORD0;
         float2 texcoord1 : TEXCOORD1;
         float2 texcoord2 : TEXCOORD2;         
      };

		struct Input {
         float2 uv_MainTex;

		  	fixed3 hl;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed _Alpha;



		//
		// float _mdpDist;
		// float _mdpTime;
		// float3 _mdpColCenter;

		float3 _mdpColCenter1;
		float3 _mdpColCenter2;
		float3 _mdpColCenter3;
		float _mdpColTime1;
		float _mdpColTime2;
		float _mdpColTime3;

		fixed _mdpEdgeTickness;
		float _mdpSpeed;
		float _mdpSpeedColor;
		float _mdpAmt;


		// fixed4 _mdpRippleColor;
		fixed4 _mdpRippleEmission;

		fixed _mdpMicVolume;
		fixed _mdpMicPitch;
		fixed2 _mdpResAmt;
		fixed2 _mdpResSpeed;
		fixed2 _mdpResScale;
		fixed4 _mdpResColorLow;
		fixed4 _mdpResColorHigh;


		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


		//RESONATE

		//hash returns [0:1]
		float3 h33(float3 p3) {
			p3 = frac(p3*float3(.1031, .1030, .0973));
		   p3 += dot(p3,p3.yxz+19.19);
		   return frac((p3.xxy + p3.yxx)*p3.zyx);
		}

		//perlin noise
		float3 n33( float3 x ) {
		   float3 p = floor(x); 
		   float3 f = frac(x);      
		   f = f*f*(3.-2.*f);
		   float n = p.x+p.y*57.+113.*p.z;      
		   return lerp(lerp(lerp( h33(n+0.), h33(n+1.),f.x),
		                    lerp( h33(n+57.), h33(n+58.),f.x),f.y),
		               lerp(lerp( h33(n+113.), h33(n+114.),f.x),
		                    lerp( h33(n+170.), h33(n+171.),f.x),f.y),f.z);
		}		

			float fbm(float3 p){

				//No FBM
					return n33(p);
			}


		//RIPPLE		
		fixed3 hsl2rgb( in fixed3 c ){
		    // fixed3 rgb = clamp( abs( mod(c.x*6.0+fixed3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0);
		    fixed3 rgb = clamp( abs( fmod(c.x*6.0+fixed3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0,1.0);
		    return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
		}

		
		fixed easeInOutQuad (fixed t) { return t<.5 ? 2*t*t : -1+(4-2*t)*t; }

		float rippleTouchOffset(float3 startPos, float3 t, float3 wp, inout fixed3 fhl){ //, out float hl

			fixed dist = t * _mdpSpeed;
			fixed l_edge = dist; //dist to edge
			fixed3 dir = wp - startPos;
			fixed l = length(dir); //dist to wp
			dir = dir/l;
			fixed l_et = _mdpEdgeTickness; //edge thickness

			float off = 0;
			float hl = 0;
			if(l>l_edge-l_et && l<l_edge+l_et){
				float x =  1.-abs((l-l_edge)/l_et) ; 
				x = easeInOutQuad( x );
				off = x;
				hl = x;
			}
			fhl.x += hl;
			fhl.y += dist;

			return off;


		}

		float3 mdpRipple(float3 wp,  float3 pt, inout float3 pn, inout fixed3 hl){


			fixed mdpResAmt = lerp(_mdpResAmt.x, _mdpResAmt.y, _mdpMicVolume);
			fixed invScale = 1./lerp(_mdpResScale.x, _mdpResScale.y, _mdpMicVolume);
			fixed mdpResSpeed = lerp(_mdpResSpeed.x, _mdpResSpeed.y, _mdpMicVolume);

			wp /= invScale;

			//RESONATE	
			float off = 0.; 
			float3 dir = float3(0,1,0);
			float _tw = _Time.y * mdpResSpeed; //warp speed
			float3 ra = float3(0,0,0);//float3(.1,.24,-.2);
			float3 rb = float3(1,1,1);	//float3(.7,-.34,.4)		
			off = fbm( (wp+ _tw*rb )   ) ; //simple perlin 
			// off = fbm( (wp+ _tw*rb*invScale ) / invScale  )*invScale ; //simple perlin 
			// off += fbm( (wp + ra + _tw*rb)*mdpResScale  )/mdpResScale ; //simple perlin 
			
			// off = (2.*off-1.); 
			float m = dot(dir, pn);
			hl.z += off*off *m* _mdpMicVolume;//*m; 
			off = 2.*(off-float(.5).xxx);//[-1,1]
			wp.xyz += off * dir *  mdpResAmt;

			wp *= invScale;
			// pn = lerp(pn, pt, off );

			// wp.xyz += off * dir * m * mdpResAmt;
			// hl.x += off*m; 
			// hl.y += off*m; 

			//RIPPLE
			float off_r = 0;
			off_r += rippleTouchOffset(_mdpColCenter1, _mdpColTime1, wp, hl); 
			off_r += rippleTouchOffset(_mdpColCenter2, _mdpColTime2, wp, hl); 
			off_r += rippleTouchOffset(_mdpColCenter3, _mdpColTime3, wp, hl); 
			off_r *= _mdpAmt;
			// off += off_r;
			wp.xyz += off_r * pn;
 
			pn = lerp(pn, pt, -off ); //*_mdpRippleNormAmt*(-1.)




			// wp.xyz += off * pn;



			return wp;
		}  



		//vertex shader
		void vert(inout appdata v, out Input o) { //

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.hl = 0;

			fixed4 wp =  mul(unity_ObjectToWorld, v.vertex); //world space point unscaled
			float3 wn =  mul(unity_ObjectToWorld, v.normal);
			float3 wt =  mul(unity_ObjectToWorld, v.tangent);

			wp.xyz = mdpRipple(wp.xyz, wt, wn, o.hl);

			wn = mul(unity_WorldToObject, wn);
			wp = mul(unity_WorldToObject, wp);

			v.vertex = float4(wp.xyz, 1.);
			v.normal = wn;

	
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			fixed3 col = hsl2rgb( fixed3(IN.hl.y*_mdpSpeedColor, 1., .5));
			//_mdpRippleColor.xyz

			o.Emission.xyz = IN.hl.x * _mdpRippleEmission * col   + IN.hl.z * lerp(_mdpResColorLow, _mdpResColorHigh, _mdpMicVolume);
			o.Albedo.xyz = IN.hl.xxx * col;//black room
			// o.Albedo.xyz =  lerp(o.Albedo, col , IN.hl.x);
			// o.Alpha = IN.hl.x;// * _Alpha;
			// o.Fade
			// o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
