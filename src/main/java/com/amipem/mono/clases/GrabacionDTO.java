package com.amipem.mono.clases;

import java.sql.Date;
import java.util.GregorianCalendar;

import javax.persistence.Temporal;

import com.amipem.mono.entidades.Grabacion;
import com.amipem.mono.entidades.Orden;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class GrabacionDTO {
	private int cantidad;

	private String codigo;



	private String tag;
	
	private int linea;
	

}
