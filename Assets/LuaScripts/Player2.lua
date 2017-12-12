import 'System'
import 'UnityEngine'
import 'Assembly-CSharp'

--[[
PROPRIEDADES DO JOGADOR
int cod 		= Codigo do jogador (1, 2, 3 ou 4)
Bullet myBullet	= Bala atirada pelo jogador
float speed 	= Velocidade de movimento
float range 	= Alcance de visao para detectar inimigos
float rangewall	= Alcance de visao para detectar paredes
int direcao 	= Direcao atual do jogador
bool atirou 	= Flag indicando se o jogador ja atirou
bool isDead 	= Flag indicando se o jogador morreu
Player target 	= Referencia ao inimigo proximo ao jogador
]]

--DIRECOES
--direita 			= 0
--direita_baixo 	= 1
--baixo 			= 2
--esquerda_baixo	= 3
--esquerda 			= 4
--esquerda_cima 	= 5
--cima 				= 6
--direita_cima 		= 7
--parado 			= 8	

function Start()

	jogador:Teste()
	jogador.direcao = 5
	jogador.range = 6
	
end

function Update()

	if jogador:SensorDeParede() then
		EsquivaParede()
	end

	if jogador:SensorDeInimigo() then
		targetPos = jogador.target:GetPos()
		targetDir = jogador:GetDirecaoTo(targetPos)
		jogador:Move(targetDir)
	end

	jogador:Move(jogador.direcao)
end

function EsquivaParede()
	jogador.direcao = (jogador.direcao + 1) % 9
end

function PosToDir(pos)
	if pos.x > 0 and pos.x < 1 then
		Debug.Log("Teste")
	end
end