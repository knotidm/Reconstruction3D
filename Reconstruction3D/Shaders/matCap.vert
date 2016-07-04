#version 130
in vec4 Position;
in vec3 Normal;

out vec2 vN;

uniform mat4 Projection;
uniform mat4 Modelview;
uniform mat3 NormalMatrix;

void main() {
    vec3 e = normalize( vec3( Modelview * Position ) );
    vec3 n = normalize( NormalMatrix * Normal );
    vec3 r = reflect( e, n );

    float m = 2.0 * sqrt(
        pow( r.x, 2.0 ) +
        pow( r.y, 2.0 ) +
        pow( r.z + 1.0, 2.0 )
    );

    vN = r.xy / m + 0.5;

    gl_Position = Projection * Modelview * Position;
}