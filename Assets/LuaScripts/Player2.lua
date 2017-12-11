import 'System'
import 'UnityEngine'
import 'Assembly-CSharp'

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
	jogador.direcao = 1
	jogador.range = 10
	
end

function Update()

	--[[if jogador:SensorDeParede() then
		EsquivaParede()
	end

	if jogador:SensorDeInimigo() then
		Debug.Log("Inimigo encontrado" .. jogador.target:GetPos().x)
	end

	jogador:Move(jogador.direcao)]]
end

function EsquivaParede()
	jogador.direcao = (jogador.direcao + 1) % 9
end

function PosToDir(position)

end