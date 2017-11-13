// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:5000,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|diff-7793-RGB,alpha-6363-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:527,x:31645,y:32599,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:_node_527,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:3342,x:31643,y:32963,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:_node_3342,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9744,x:31862,y:32924,varname:node_9744,prsc:2,ntxv:0,isnm:False|TEX-3342-TEX;n:type:ShaderForge.SFN_Tex2d,id:7793,x:32242,y:32567,varname:node_7793,prsc:2,ntxv:0,isnm:False|UVIN-3275-UVOUT,TEX-527-TEX;n:type:ShaderForge.SFN_OneMinus,id:5098,x:32092,y:32935,varname:node_5098,prsc:2|IN-9744-R;n:type:ShaderForge.SFN_ValueProperty,id:9343,x:32020,y:32773,ptovrint:False,ptlb:Alpha tranparancy,ptin:_Alphatranparancy,varname:_node_9343,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:6363,x:32446,y:32851,varname:node_6363,prsc:2|A-9343-OUT,B-5098-OUT;n:type:ShaderForge.SFN_Panner,id:3637,x:32084,y:32132,varname:node_3637,prsc:2,spu:1,spv:0|UVIN-7045-UVOUT,DIST-5246-OUT;n:type:ShaderForge.SFN_TexCoord,id:7045,x:31588,y:32089,varname:node_7045,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:3128,x:31553,y:32304,varname:node_3128,prsc:2;n:type:ShaderForge.SFN_Panner,id:3275,x:32307,y:32259,varname:node_3275,prsc:2,spu:0,spv:1|UVIN-3637-UVOUT,DIST-1610-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9132,x:31752,y:32191,ptovrint:False,ptlb:X pan,ptin:_Xpan,varname:node_9132,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4027,x:31875,y:32404,ptovrint:False,ptlb:Y pan,ptin:_Ypan,varname:node_4027,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5246,x:31925,y:32186,varname:node_5246,prsc:2|A-9132-OUT,B-3128-TSL;n:type:ShaderForge.SFN_Multiply,id:1610,x:32099,y:32343,varname:node_1610,prsc:2|A-3128-TSL,B-4027-OUT;proporder:527-3342-9343-4027-9132;pass:END;sub:END;*/

Shader "Shader Forge/fogS" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", 2D) = "black" {}
        _Alphatranparancy ("Alpha tranparancy", Float ) = 0.5
        _Ypan ("Y pan", Float ) = 0
        _Xpan ("X pan", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+5000"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
			ZTest Always
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float _Alphatranparancy;
            uniform float _Xpan;
            uniform float _Ypan;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_3128 = _Time;
                float2 node_3275 = ((i.uv0+(_Xpan*node_3128.r)*float2(1,0))+(node_3128.r*_Ypan)*float2(0,1));
                float4 node_7793 = tex2D(_Texture,TRANSFORM_TEX(node_3275, _Texture));
                float3 diffuseColor = node_7793.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float4 node_9744 = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                return fixed4(finalColor,(_Alphatranparancy*(1.0 - node_9744.r)));
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float _Alphatranparancy;
            uniform float _Xpan;
            uniform float _Ypan;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_3128 = _Time;
                float2 node_3275 = ((i.uv0+(_Xpan*node_3128.r)*float2(1,0))+(node_3128.r*_Ypan)*float2(0,1));
                float4 node_7793 = tex2D(_Texture,TRANSFORM_TEX(node_3275, _Texture));
                float3 diffuseColor = node_7793.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float4 node_9744 = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                return fixed4(finalColor * (_Alphatranparancy*(1.0 - node_9744.r)),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
