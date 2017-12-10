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

direcao = nil

function Start()

	direcao = 0
	
end

function Update()

	--if jogador:SensorDeInimigo() ~= nil then
	--	inimigo = jogador:SensorDeInimigo()		
	--	posicaoInimigo = inimigo:GetPos()
	--end

	if jogador:SensorDeParede() then
		direcao = (direcao + 1) % 9
	end

	Move(direcao)

end

function VerInimigo()

	

end