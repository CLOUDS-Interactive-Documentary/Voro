#version 120

uniform sampler2DRect tex;
uniform vec2 resolution;
                                  
vec2 texel = vec2(1.0 / resolution.x, 1.0 / resolution.y);

// TODO: hard code matrix values!!!!
mat3 G[9] = mat3[](1.0/(2.0*sqrt(2.0)) * mat3( 1.0, sqrt(2.0), 1.0, 0.0, 0.0, 0.0, -1.0, -sqrt(2.0), -1.0 ),
                1.0/(2.0*sqrt(2.0)) * mat3( 1.0, 0.0, -1.0, sqrt(2.0), 0.0, -sqrt(2.0), 1.0, 0.0, -1.0 ),
                1.0/(2.0*sqrt(2.0)) * mat3( 0.0, -1.0, sqrt(2.0), 1.0, 0.0, -1.0, -sqrt(2.0), 1.0, 0.0 ),
                1.0/(2.0*sqrt(2.0)) * mat3( sqrt(2.0), -1.0, 0.0, -1.0, 0.0, 1.0, 0.0, 1.0, -sqrt(2.0) ),
                1.0/2.0 * mat3( 0.0, 1.0, 0.0, -1.0, 0.0, -1.0, 0.0, 1.0, 0.0 ),
                1.0/2.0 * mat3( -1.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, -1.0 ),
                1.0/6.0 * mat3( 1.0, -2.0, 1.0, -2.0, 4.0, -2.0, 1.0, -2.0, 1.0 ),
                1.0/6.0 * mat3( -2.0, 1.0, -2.0, 1.0, 4.0, 1.0, -2.0, 1.0, -2.0 ),
                1.0/3.0 * mat3( 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 ));

void main(void)
{
    mat3 I;
    float cnv[9];
    vec3 sample;
    
    /* fetch the 3x3 neighbourhood and use the RGB vector's length as intensity value */
    for (int i=0; i<3; i++)
    {
        for (int j=0; j<3; j++)
        {
            sample = texture2DRect(tex, gl_TexCoord[0].st + vec2(i-1.0,j-1.0)).rgb;
            I[i][j] = length(sample); 
        }
    }

    /* calculate the convolution values for all the masks */
    for (int i=0; i<9; i++)
    {
        float dp3 = dot(G[i][0], I[0]) + dot(G[i][1], I[1]) + dot(G[i][2], I[2]);
        cnv[i] = dp3 * dp3; 
    }

    float M = (cnv[0] + cnv[1]) + (cnv[2] + cnv[3]);
    float S = (cnv[4] + cnv[5]) + (cnv[6] + cnv[7]) + (cnv[8] + M); 

    gl_FragColor = vec4(vec3(sqrt(M/S)), 1.0);
}
