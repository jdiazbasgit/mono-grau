package com.amipem.mono.entidades;

import java.util.GregorianCalendar;

import javax.persistence.*;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Entity
@Table(name = "grabaciones")
public class Grabacion {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private int id;
	@ManyToOne
	@JoinColumn(name="orden_id")
	private Orden orden;
	private String tag;
	private int lote;
	private int linea;

	
	
}
