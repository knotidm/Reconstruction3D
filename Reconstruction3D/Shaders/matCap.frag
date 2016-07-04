#version 130
#ifdef GL_ES
precision mediump float;
precision mediump int;
#endif

in vec2 vN;
out vec4 FragColor;

uniform sampler2D texture;

void main() {
    vec3 base = texture2D( texture, vN ).rgb;
    FragColor = vec4( base, 1. );
}